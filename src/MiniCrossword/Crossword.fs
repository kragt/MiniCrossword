namespace MiniCrossword

module Crossword =
    open Avalonia.Controls
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int }
    let init = { count = 0 }

    type Msg = Increment | Decrement | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init

    let rSize = 100.0
    let rLeft = 40.0
    let rTop = 65.0
    let rStroke = 2.0
    let calcLeft i = rLeft + (rSize - rStroke) * float i
    let rect n = 
        [
            for i in 0..n do 
                Rectangle.create [
                    Rectangle.stroke "Black"
                    Rectangle.strokeThickness rStroke
                    Rectangle.left (calcLeft i)
                    Rectangle.top rTop
                    Rectangle.width rSize
                    Rectangle.height rSize
                ] :> Avalonia.FuncUI.Types.IView
        ]

    let header = 
        TextBlock.create [
            TextBlock.foreground "Black"
            TextBlock.left 20.0
            TextBlock.top 20.0
            TextBlock.fontSize 18.0
            TextBlock.verticalAlignment VerticalAlignment.Center
            TextBlock.horizontalAlignment HorizontalAlignment.Left
            TextBlock.text "Crossword"
        ] :> Avalonia.FuncUI.Types.IView

    let canvasChildren = header :: (rect 6)
        
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Canvas.create [
                    Canvas.dock Dock.Top
                    Canvas.background "White"
                    Canvas.children canvasChildren
                ]
            ]
        ]       

//let view (state: State) (dispatch: Msg -> unit) =
//    DockPanel.create [
//        DockPanel.children [
//            TreeView.create [
//                TreeView.dock Dock.Left
//                TreeView.viewItems [       
//                    for entity in state.Entries do
//                        createView dispatch entity
//                ]
//            ]
//        ]
//    ]