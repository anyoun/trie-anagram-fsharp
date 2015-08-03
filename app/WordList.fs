module WordList

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Text.RegularExpressions
open Config

type Word = {
  Word : string;
  //Rarity : int;
  //Category : string;
  //Subcategory : string;
}

let basePath = @"scowl_word_lists"

let readAllFiles () =
        let crossJoin xList yList = xList |> Seq.collect (fun x -> yList |> Seq.map (fun y -> (x,y)))
        [ 10; 20; 35; 40; 50; 55; 60; 70; 80; 95  ]
        |> Seq.filter (fun s -> s <= Config.MaxWordListSize)
        |> crossJoin Config.IncludedCategories
        |> crossJoin Config.IncludedSubcategories
        |> Seq.map (fun (subcategory, (category, size)) -> (category, subcategory, size, Path.Combine(basePath, (sprintf @"%s-%s.%d" category subcategory size))))
        |> Seq.filter (fun (_,_,_, path) -> File.Exists path)
        |> Seq.collect (fun (category, subcategory, size, path) ->
            if Config.TraceLookup then
              printfn "Reading %s" path
            File.ReadLines path
            |> Seq.filter Config.AllowWord
            |> Seq.map (fun line -> {
                                      Word = line.Trim().ToLower();
                                      //Rarity= size;
                                      //Category=category;
                                      //Subcategory=subcategory
                                    } ))

let getCharFreq words =
    words
    |> Seq.collect (fun w -> w.Word)
    |> Seq.groupBy (fun c -> c)
    |> Map.ofSeq
    |> Map.map (fun k v -> Seq.length v)

let AllWords = readAllFiles ()
let CharFreq = getCharFreq AllWords
