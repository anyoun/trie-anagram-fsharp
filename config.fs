module Config
    open System
    
    let MaxWordListSize = 80
    let IncludedCategories = [
        "english";
        "american";
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
    let AllowWord (word:string) =
        not (
                word.Contains("'")
            ||  word.Contains("?")
            || (word.Length = 1 && not (allowed_one_letter.Contains word))
            || (word.Length = 2 && not (allowed_two_letter.Contains word))
        )
    exception UnknownCharacter of string
    let LetterToIndex c = (int (Char.ToUpper c)) - (int 'A')