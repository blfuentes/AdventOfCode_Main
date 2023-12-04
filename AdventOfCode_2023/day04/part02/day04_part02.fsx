#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023.Modules

type CardSet = {
    CardId: int
    WinningCards: int array
    MyCards: int array
    IsOriginal: bool
}

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let lines = Utilities.GetLinesFromFile path

let buildCardSet (input: string) =
    let parts = input.Split(':')
    let cardId = Regex.Match(parts.[0], @"\d+").Value |> int
    let winningCards = Regex.Matches(parts.[1].Split('|').[0], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.distinct |> Seq.toArray
    let myCards = Regex.Matches(parts.[1].Split('|').[1], @"\d+") |> Seq.map _.Value |> Seq.map int |> Seq.distinct |> Seq.toArray
    { CardId = cardId; MyCards = myCards; WinningCards = winningCards; IsOriginal = true}

let countMatchingCards (cardSet: CardSet) =
    cardSet.MyCards |> Seq.filter (fun x -> cardSet.WinningCards |> Seq.contains x) |> Seq.length

let getPoints (input: int) =
    match input with
    | v when v > 0 -> Math.Pow(2.0, float (v - 1)) |> int
    | _ -> 0

let cardSet (input: string array) =
    input |> Seq.map buildCardSet |> Seq.toList

let rec playCardSet (originalCardSet: CardSet list) (remainingCardSet: CardSet list) =
    match remainingCardSet with
    | [] -> originalCardSet
    | card :: tail ->
        let matchingCards = countMatchingCards card
        let copyCardsIds = [card.CardId + 1 .. card.CardId + matchingCards]
        let copyCards = originalCardSet |> Seq.filter (fun x -> copyCardsIds |> Seq.contains x.CardId) |> Seq.toList
        let produceCopies = copyCards |> Seq.map countMatchingCards |> Seq.sum > 0
        match produceCopies with
        | false ->
            playCardSet originalCardSet tail
        | true ->
            playCardSet (originalCardSet @ copyCards) (tail @ copyCards)

let finalBoard = (playCardSet (cardSet lines) (cardSet lines)) |> List.groupBy _.CardId
let result = finalBoard |> List.sumBy (fun b -> (snd b).Length)
printfn "Result %i" result