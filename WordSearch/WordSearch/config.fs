module Config

open System
    
let MaxWordListSize = 95
let IncludedCategories = [
    "english";
    "american";
    "british";
    //"canadian";
]
let IncludedSubcategories = [
    // "abbreviations";
    // "contractions";
    "proper-names";
    "upper";
    "words";
]
let allowed_one_letter = set [ "a"; "i"; ]
let allowed_two_letter = set [
                                "ah";
                                "al";
                                "by";
                                "de";
                                "do";
                                "ex";
                                "em";
                                "en";
                                "hi";
                                "ha";
                                "ho";
                                "if";
                                "in";
                                "is";
                                "it";
                                "ma";
                                "my";
                                "oh";
                                "on";
                                "or";
                                "ox";
                                "un";
                                "us";
                                "we";
                                ]
let LetterToIndex c = (int (Char.ToUpper c)) - (int 'A')
let hasBadChars word = word |> Seq.exists (fun ch ->
    let index = LetterToIndex ch
    index < 0 || index >= 26) 

let AllowWord (word:string) =
    not (
            word.Contains("'")
        ||  word.Contains("?")
        || (word.Length = 1 && not (allowed_one_letter.Contains word))
        || (word.Length = 2 && not (allowed_two_letter.Contains word))
        || (hasBadChars word)
    )
exception UnknownCharacter of string
