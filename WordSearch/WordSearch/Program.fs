module Program

[<EntryPoint>]
let main argv =
//    printfn "%A" argv
    //WordSearch.RunWordSearch ()
    let trie = Trie.BuildTrie WordList.AllWords
    let res = Anagram.anagram trie "dormitory" 0
    for r in res do
      for w in r do
        printfn "%s" w.Word
    0
