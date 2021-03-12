namespace MiniCrossword

module Shell =
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish
    open BubbleMsg

    type State = { 
            AboutState: About.State
            CrosswordState: Crossword.State
            SettingsState: Settings.State
            //crossword
        }

    type Msg =
        | AboutMsg of About.Msg
        | CrosswordMsg of Crossword.Msg
        | SettingsMsg of Settings.Msg

    let init =
        let aboutState, aboutCmd = About.init
        let crosswordState = Crossword.init
        let settingsState = Settings.init
        { AboutState = aboutState
          CrosswordState = crosswordState
          SettingsState = settingsState },
        Cmd.batch [ aboutCmd ]

    let private handleBubbleMsg (state: Settings.State) (extMsg: BubbleMsg option) : Cmd<Msg> =
        match extMsg with
        | None -> Cmd.none
        | Some msg -> 
            match msg with
            | BubbleMsg.NavigateToPage p -> Cmd.none

    let update (msg: Msg) (state: State): State * Cmd<_> =
        match msg with
        | AboutMsg bpmsg ->
            let aboutState, cmd =
                About.update bpmsg state.AboutState
            { state with AboutState = aboutState },
            Cmd.map AboutMsg cmd
        | CrosswordMsg crosswordmsg ->
            let crosswordMsg =
                Crossword.update crosswordmsg state.CrosswordState
            { state with CrosswordState = crosswordMsg },
            Cmd.none
        | SettingsMsg settingsMgs ->
            let settingsState, cmd, bubble = Settings.update settingsMgs state.SettingsState
            let bubbled = handleBubbleMsg settingsState bubble
            let chained = Cmd.map SettingsMsg cmd
            { state with SettingsState = settingsState }, Cmd.batch [ bubbled; chained ]

    let view (state: State) (dispatch) =
        DockPanel.create [ 
            DockPanel.children [ 
                TabControl.create [ 
                    TabControl.tabStripPlacement Dock.Top
                    TabControl.viewItems [
                        TabItem.create [ 
                            TabItem.header "Crossword Sample"
                            TabItem.content (Crossword.view state.CrosswordState (CrosswordMsg >> dispatch))
                        ]
                        TabItem.create [ 
                            TabItem.header "About"
                            TabItem.content (About.view state.AboutState (AboutMsg >> dispatch))
                        ]
                        TabItem.create [
                            TabItem.header "Settings"
                            TabItem.content (Settings.view state.SettingsState (SettingsMsg >> dispatch))
                        ]
                    ]
                ]
            ]
        ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Full App"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
