module Program

open System
open System.Threading
open ServiceStack

type Hello = { mutable Name: string; }
type HelloResponse = { mutable Result: string; }
type HelloService() =
    interface IService
    member this.Any (req:Hello) = { Result = "Hello, " + req.Name }

let CachedTrie = Trie.BuildTrie WordList.AllWords

type AnagramRequest = { mutable Name: string; }
type AnagramResponse = { mutable Result: array<string>; }
type AnagramService() =
    interface IService
    member this.Any (req:AnagramRequest) =
      { Result =
        Anagram.anagram CachedTrie req.Name 0
          |> Seq.sortBy (fun r -> -r.Count)
          |> Seq.choose (fun r ->
            let matchLen = r |> Seq.sumBy (fun w -> w.Word.Length)
            if matchLen = req.Name.Length then
              Some (r |> Seq.map (fun w -> w.Word) |> String.concat " ")
            else
              None)
          |> Array.ofSeq
       }

//Define the Web Services AppHost
type AppHost =
    inherit AppSelfHostBase
    new() = { inherit AppSelfHostBase("Hello F# Services", typeof<HelloService>.Assembly) }
    override this.Configure container =
        base.Routes
            .Add<Hello>("/hello")
            .Add<Hello>("/hello/{Name}")
            .Add<AnagramRequest>("/anagram/{Name}")
            |> ignore

//Run it!
[<EntryPoint>]
let main args =
    let host = if args.Length = 0 then "http://*:1337/" else args.[0]
    printfn "listening on %s ..." host
    let appHost = new AppHost()
    ignore (appHost.Init())
    ignore (appHost.Start host)
    //Console.ReadLine() |> ignore
    let evt = new ManualResetEvent false

    Console.CancelKeyPress.Add (fun e ->
        e.Cancel <- true
        ignore (evt.Set ()) )

    printfn "Running..."
    ignore (evt.WaitOne())
    0
