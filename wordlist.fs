module WordList
    open System
    open System.Collections.Generic
    open System.IO
    open System.Text
    open System.Text.RegularExpressions

    let basePath = @"c:\Users\will.thomas\Documents\Github\trie-anagram\scowl_word_lists\"
    
    let readAllFiles () =
         let crossJoin xList yList = xList |> Seq.collect (fun x -> yList |> Seq.map (fun y -> (x,y)))
         [ 10; 20; 35; 40; 50; 55; 60; 70; 80; 95  ]
            |> Seq.filter (fun s -> s <= Config.MaxWordListSize)
            |> crossJoin Config.IncludedCategories
            |> crossJoin Config.IncludedSubcategories
            |> Seq.map (
                fun (subcategory, (category, size)) -> Path.Combine(basePath, (sprintf @"%s-%s.%d" category subcategory size)))
            |> Seq.filter File.Exists
            |> Seq.collect File.ReadLines
            |> Seq.map (fun word -> word.Trim().ToLower())
            |> Seq.filter Config.AllowWord
            |> Seq.filter (fun word ->
                let bad = word |> Seq.exists (fun ch ->
                    let index = Config.LetterToIndex ch
                    index < 0 || index >= 26) 
                if bad then
                    printfn "Ignoring word \"%s\"" word
                    //let badString = Seq.fold (fun s c -> s+(string c)) "" bad
                    //raise (Config.UnknownCharacter (sprintf "Bad characters: \"%s\" in \"%s\"." badString word) )
                    false  
                else
                    true
                )
    
    // let words = query {
    //     for l in File.ReadLines @"c:\Users\will.thomas\Downloads\hunspell-en_US-2015.05.18\en_US.dic" do
    //     let m = Regex.Match(l, @"^([a-z]+)(/.*)$")
    //     where (m.Success)
    //     let word = m.Groups.[1].Value.ToLower()
    //     select word
    // }
    let AllWords = readAllFiles () 
        