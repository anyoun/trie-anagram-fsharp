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
let crossUnion3 xSet ySet =
  Set (seq {
    for x in xSet do
      if Set.count ySet = 0 then
        yield x
      else
        for y in ySet do
          yield (Set.union x y)
    })

let rec findAnagrams (root:Trie.Node) (node:Trie.Node option) (nextChars:list<char>) (skippedChars:list<char>) wildCardsLeft (path:list<char>) : Set<Set<Word>> =
  let depth = List.length path
  if depth = 100 then
    raise (System.Exception "Depth is too large")
  if Config.TraceLookup then
    printfn "%sAt: %s Left: %s Skipped %s" (String.replicate depth " ") (Config.ListToString path) (string nextChars) (string skippedChars)
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
        printfn "%sFound: %s" (String.replicate depth " ") words
      let res = findAnagrams root (Some root) newCharNode [] wildCardsLeft ('/'::path)
      newWordSets <- crossUnion3 newWords res

    if wildCardsLeft > 0 then
      for (ch,n) in n.ChildPairs do
        newWordSets <- Set.union newWordSets (findAnagrams root node nextChars skippedChars (wildCardsLeft-1) (ch::path))

  match nextChars with
  | [] -> ()
  | x::xs ->
    //Search by skipping
    newWordSets <- Set.union newWordSets (findAnagrams root node xs (x::skippedChars) wildCardsLeft ('_'::path))

    match node with
    | None -> ()
    | Some n ->
      let nextNode = n.[x]
      if Config.TraceLookup then
        printfn "%sMatched char %c" (String.replicate depth " ") x
      newWordSets <- Set.union newWordSets (findAnagrams root nextNode xs skippedChars wildCardsLeft (x::path))

  newWordSets

let anagram root chars wildCards =
  findAnagrams root (Some root) (List.ofSeq chars |> List.sort) [] wildCards []
