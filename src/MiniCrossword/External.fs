namespace MiniCrossword

module External =
    
    type Page =
    | Crossword
    | Settings
    | WordList

    let getPageNo i =
        match i with
        | Crossword -> 1
        | Settings -> 2
        | WordList -> 3

    type ExternalMsg =
    | NavigateToPage of Page
