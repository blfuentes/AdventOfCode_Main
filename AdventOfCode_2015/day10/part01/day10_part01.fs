module day10_part01

let rec lookAndSay (round: int) (max: int) (input: string) : string =
    let rec lookAndSay' (consumed: string) (input: string list) : string =
        if input.IsEmpty then consumed
        else
            let initial = input |> List.takeWhile(fun x -> x = input.Head)
            let remaining = input |> List.skipWhile(fun x -> x = input.Head)
            let consumed' = initial.Length.ToString() + input.Head
            lookAndSay' (consumed + consumed') remaining
    if round = max then input
    else
        lookAndSay (round + 1) max (lookAndSay' "" (input.ToCharArray() |> Array.map string |> List.ofArray))

let execute =
    (lookAndSay 0 40 "1321131112").Length