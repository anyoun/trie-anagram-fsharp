open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Text.RegularExpressions

let MAX_DEPTH = 10
let full = @"
    npwdesLuva
    yaxehhcsdy
    knidemetit
    lahsdnnssi
    omjmeuiiaL
    neesLtdLfi
    gLfitjrLfn
    reeLieaaee
    bsoatnpdcv
    ssdmngoetu
    tLveememsj
    uyLketLmqi
    vehcatraeh
    qdessapybz
    famtnadixo"
let pad = Array2D.create 15 10 ' '
let padT =  Array2D.create 10 15 ' '
let words = query {
    for l in File.ReadLines @"c:\Users\will.thomas\Downloads\hunspell-en_US-2015.05.18\en_US.dic" do
    let m = Regex.Match(l, @"^([a-z]+)(/.*)$")
    where (m.Success)
    let word = m.Groups.[1].Value.ToLower()
    select word
}

type Trie() =
    let children = Array.create<Trie option> 26 None
    let cvt c = (int (Char.ToUpper c)) - (int 'A')
    member this.Item
        with get(ch) = children.[cvt(ch)]
        and set ch value = children.[cvt(ch)] <- value
    member val Words = new HashSet<string>()
    
let BuildTrie wordList = 
    let root = new Trie()
    let mutable wordCount = 0
    for word in wordList do
        let mutable n = root
        for c in word do
            n <- 
                match n.[c] with
                | None -> 
                    let x = new Trie()
                    n.[c] <- Some x
                    x
                | Some x -> x
        if n.Words.Add word then
            wordCount <- wordCount + 1
    printfn "Loaded %d words" wordCount
    root
    
let matchedWords = new HashSet<string>()
    
let rec SearchTrie (t:Trie) r c d =
    if r >= 0 && c >= 0 && r < pad.GetLength(0) && c < pad.GetLength(1) then
        let nextCh = pad.[r,c]
        match t.[nextCh] with
        | None -> ()
        | Some newWord ->
            for w in newWord.Words do
                ignore (matchedWords.Add w)
            
            SearchTrie newWord (r-1) (c-1) (d+1)
            SearchTrie newWord (r-1) (c)   (d+1)
            SearchTrie newWord (r-1) (c+1) (d+1)
            SearchTrie newWord (r)   (c-1) (d+1)
            SearchTrie newWord (r)   (c+1) (d+1)
            SearchTrie newWord (r+1) (c-1) (d+1)
            SearchTrie newWord (r+1) (c)   (d+1)
            SearchTrie newWord (r+1) (c+1) (d+1)
            
let rec SearchTrieOneDirection (t:Trie) r c rDelta cDelta =
    if r >= 0 && c >= 0 && r < pad.GetLength(0) && c < pad.GetLength(1) then
        let nextCh = pad.[r,c]
        match t.[nextCh] with
        | None -> ()
        | Some newWord ->
            for w in newWord.Words do
                ignore (matchedWords.Add w)
            SearchTrieOneDirection newWord (r+rDelta) (c+cDelta) rDelta cDelta
let rec SearchTrieAllDirections (t:Trie) r c =
    SearchTrieOneDirection t r c (-1) (-1)
    SearchTrieOneDirection t r c (-1) (0)  
    SearchTrieOneDirection t r c (-1) (+1)
    SearchTrieOneDirection t r c (0)   (-1)
    SearchTrieOneDirection t r c (0)   (+1)
    SearchTrieOneDirection t r c (+1) (-1)
    SearchTrieOneDirection t r c (+1) (0)  
    SearchTrieOneDirection t r c (+1) (+1)


let freq = new Dictionary<char, int>()
let mutable r = 0
let mutable count = 0
for line in full.Split([|'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries) do
    let mutable c = 0
    for ch in line.Trim() do
        let ch2 = Char.ToLower(ch)
        pad.[r, c] <- ch2
        padT.[c,r] <- ch2
        c <- c + 1
        freq.[ch2] <- 1 + (if freq.ContainsKey(ch2) then freq.[ch2] else 0)
        count <- count + 1
    r <- r + 1

let trie = BuildTrie words
for x = 0 to pad.GetLength(0) do
    for y = 0 to pad.GetLength(1) do
        //SearchTrie trie x y 0
        SearchTrieAllDirections trie x y 

let sorted = 
    matchedWords
    |> Seq.filter (fun w -> w.Length >= 6 && w.Length <= 10)
    |> Seq.sortBy (fun w -> w.Length)

printfn "Matched %d words total, %d filtered" matchedWords.Count (Seq.length sorted)
for w in sorted do
    printfn "%s" w

