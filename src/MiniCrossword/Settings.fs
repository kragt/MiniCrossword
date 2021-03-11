namespace MiniCrossword

module Settings =

    open Elmish
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    type Bounds = {
        KeywordLower: int
        KeywordUpper: int
        WordLower: int
        WordUpper: int
    }
    
    type State = {
        Current: Bounds
        Saved: Bounds
    }

    let initialSettings = {
        KeywordLower = 6
        KeywordUpper = 8
        WordLower = 3
        WordUpper = 6
    }

    let init = {
        Current = initialSettings
        Saved = initialSettings
    }

    type Msg =
    | SetKeywordLower of string
    | SetKeywordUpper of string
    | SetWordLower of string
    | SetWordUpper of string
    | Save
    | Reset

    let parseIntOrDefault (str : string) d =
        match System.Int32.TryParse str with
        | true, i -> i
        | false, _ -> d

    let update (msg: Msg) (state: State) =
        match msg with
        | SetKeywordLower s -> 
            let current = { state.Current with KeywordLower = parseIntOrDefault s state.Current.KeywordLower }
            { state with Current = current }, Cmd.none, None
        | SetKeywordUpper s -> 
            let current = { state.Current with KeywordUpper = parseIntOrDefault s state.Current.KeywordUpper }
            { state with Current = current }, Cmd.none, None
        | SetWordLower s -> 
            let current = { state.Current with WordLower = parseIntOrDefault s state.Current.WordLower }
            { state with Current = current }, Cmd.none, None
        | SetWordUpper s -> 
            let current = { state.Current with WordUpper = parseIntOrDefault s state.Current.WordUpper }
            { state with Current = current }, Cmd.none, None
        | Save ->
            { state with Saved = state.Current }, Cmd.none, None
        | Reset ->
            { state with Current = state.Saved }, Cmd.none, None

    let view (state: State) dispatch =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Top
                    StackPanel.margin 10.0
                    StackPanel.spacing 10.0
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.text "Settings page"
                        ]
                        StackPanel.create [
                            StackPanel.children [
                                TextBlock.create [
                                    TextBlock.text "Keyword"
                                ]
                                TextBox.create [
                                    TextBox.text (string state.Current.KeywordLower)
                                    TextBox.onTextChanged (SetKeywordLower >> dispatch)
                                ]
                                TextBox.create [
                                    TextBox.text (string state.Current.KeywordUpper)
                                    TextBox.onTextChanged (SetKeywordUpper >> dispatch)
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.children [
                                TextBlock.create [
                                    TextBlock.text "Words"
                                ]
                                TextBox.create [
                                    TextBox.text (string state.Current.WordLower)
                                    TextBox.onTextChanged (SetWordLower >> dispatch)
                                ]
                                TextBox.create [
                                    TextBox.text (string state.Current.WordUpper)
                                    TextBox.onTextChanged (SetWordUpper >> dispatch)
                                ]
                            ]
                        ]
                        Button.create [
                            Button.content "Cancel"
                            Button.padding (20.0, 10.0)
                            Button.onClick (fun _ -> dispatch Reset)
                        ]
                        Button.create [
                            Button.content "OK"
                            Button.padding (20.0, 10.0)
                            Button.onClick (fun _ -> dispatch Save)
                        ]
                    ]
                ]
            ]
        ]