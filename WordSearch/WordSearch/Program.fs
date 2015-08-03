module Program

[<EntryPoint>]
let main argv =
//    printfn "%A" argv
    //WordSearch.RunWordSearch ()
    let trie = Trie.BuildTrie WordList.AllWords
    //Trie.DumpTrie trie
    let target = argv.[0]
    let res = Anagram.anagram trie target 0
    for r in res do
      let matchLen = r |> Seq.sumBy (fun w -> w.Word.Length)
      if matchLen = target.Length then
        printf "%s" (r
          |> Seq.map (fun w -> w.Word)
          |> String.concat " ")
        printfn ""
    0
