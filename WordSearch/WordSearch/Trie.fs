module Trie
open System
open System.Collections.Generic
open WordList

type Node() =
    let children = Array.create<Node option> 26 None
    member this.Item
        with get(ch) = children.[Config.LetterToIndex(ch)]
        and set ch value = children.[Config.LetterToIndex(ch)] <- value
    member val Words = new System.Collections.Generic.HashSet<Word>()
    member this.ChildPairs = children |> Array.mapi (fun i n -> (Config.IndexToLetter i),n)
    member this.Children = children |> Seq.choose (fun x -> x)

let rec DumpTrie2 (n:Node) ch depth =
  let indent = (String.replicate depth " ")
  let words = n.Words |> Seq.map (fun w -> w.Word) |> String.concat " "
  printfn "%s%c: %s" indent ch words
  for (ch,child) in n.ChildPairs do
    match child with
    | None -> ()
    | Some x -> DumpTrie2 x ch (depth+1)
let DumpTrie n =
  DumpTrie2 n '*' 0

let PrepWord s =
  s |> Seq.sort
  //s |> Seq.sortBy (fun c -> WordList.CharFreq.[c])

let BuildTrie (wordList:seq<Word>) =
    let root = new Node()
    let mutable totalWordCount = 0
    let mutable uniqueWordCount = 0
    for word in wordList do
        let mutable n = root
        for c in (PrepWord word.Word) do
            n <-
                match n.[c] with
                | None ->
                    let x = new Node()
                    n.[c] <- Some x
                    x
                | Some x -> x
        totalWordCount <- totalWordCount + 1
        if n.Words.Add word then
            uniqueWordCount <- uniqueWordCount + 1
    printfn "Loaded %d words, %d unique" totalWordCount uniqueWordCount
    root
