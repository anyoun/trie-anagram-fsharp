module Trie
open System
open System.Collections.Generic
open WordList

type Node() =
    let children = Array.create<Node option> 26 None
    member this.Item
        with get(ch) = children.[Config.LetterToIndex(ch)]
        and set ch value = children.[Config.LetterToIndex(ch)] <- value
    member val Words = new HashSet<Word>()
    member this.Children = children |> Seq.choose (fun x -> x) 
    
let BuildTrie (wordList:seq<Word>) = 
    let root = new Node()
    let mutable wordCount = 0
    for word in wordList do
        let mutable n = root
        for c in word.Word do
            n <- 
                match n.[c] with
                | None -> 
                    let x = new Node()
                    n.[c] <- Some x
                    x
                | Some x -> x
        if n.Words.Add word then
            wordCount <- wordCount + 1
    printfn "Loaded %d words" wordCount
    root