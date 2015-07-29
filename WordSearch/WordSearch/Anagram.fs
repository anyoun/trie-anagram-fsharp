module Anagram

open Config
open WordList
open Trie

let combineSortChars x y =
  List.concat [ x; y ] |> List.sort
let crossUnion xSet ySet =
  let mutable r = Set.empty
  for x in xSet do
    for y in ySet do
      r <- r.Add (Set.union x y)
  r
let crossJoin xList yList = xList |> Seq.collect (fun x -> yList |> Seq.map (fun y -> (x,y)))
let crossUnion2 xSet ySet =
  crossJoin xSet ySet
  |> Seq.map (fun (x,y) -> Set.union x y)

let rec findAnagrams (root:Trie.Node) (node:Trie.Node option) (nextChars:list<char>) (skippedChars:list<char>) wildCardsLeft depth =
  if depth = 100 then
    raise (System.Exception "Depth is too large")
  if Config.TraceLookup then
    printfn "%sLeft: %s Skipped %s" (String.replicate depth " ") (string nextChars) (string skippedChars)
  let originalNode = node
  let remainingStr = ""
  let skippedStr = ""
  let mutable newWordSets = Set.empty
  match node with
  | None -> ()
  | Some n ->
    if n.Words.Count > 0 then
      let newCharNode = combineSortChars nextChars skippedChars
      let newWords = Set ( n.Words |> Seq.map (fun w -> Set [w]) )
      if Config.TraceLookup then
        let words = n.Words |> Seq.map (fun w -> w.Word) |> String.concat " "
        printfn "%sFound: %s, continuing with %s" (String.replicate depth " ")  words (string newCharNode)
      let res = findAnagrams root (Some root) newCharNode [] wildCardsLeft (depth+1)
      newWordSets <- crossUnion newWords res

    if wildCardsLeft > 0 then
      for n in n.Children do
        newWordSets <- Set.union newWordSets (findAnagrams root node nextChars skippedChars (wildCardsLeft-1) (depth+1))

  match nextChars with
  | [] -> ()
  | x::xs ->
    //Search by skipping
    newWordSets <- Set.union newWordSets (findAnagrams root node xs (x::skippedChars) wildCardsLeft (depth+1))

    match node with
    | None -> ()
    | Some n ->
      let nextNode = n.[x]
      if Config.TraceLookup then
        printfn "%sMatched char %c" (String.replicate depth " ") x
      newWordSets <- Set.union newWordSets (findAnagrams root nextNode xs skippedChars wildCardsLeft (depth+1))

  newWordSets

let anagram root chars wildCards =
  findAnagrams root (Some root) (List.ofSeq chars |> List.sort) [] wildCards 0
