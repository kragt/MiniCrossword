namespace MiniCrossword

module BubbleMsg =
    
    type Page =
    | Crossword
    | Settings
    | WordList

    let getPageNo i =
        match i with
        | Crossword -> 0
        | Settings -> 1
        | WordList -> 2

    type BubbleMsg =
    | NavigateToPage of Page
