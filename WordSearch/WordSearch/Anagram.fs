module Anagram

let findAnagrams root:Trie.Node (node:Trie.Node option) nextChars wildCardsLeft (foundWordSets:set<set<WordList.Word>>) (currentWordSets:set<set<WordList.Word>>) depth=
    match nextChars with
    | [] -> []
    | match node with
        | None -> []
        | Some node -> 
            if (Seq.exists (fun x -> x) node.Words) then
            let nextWordSets = []
                for cws in currentWordSets do
                    for w in node.Words do
                        nextWordSets.Add(cws.union([word]))
            if wildCardsLeft > 0 then
                for n in node.Children do
                    findAnagrams
