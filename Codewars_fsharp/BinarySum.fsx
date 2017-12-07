
let BinarySum a b =
    let rec intToBinary i =
        match i with
        | 0 | 1 -> string i
        | _ ->
            let bit = string (i % 2)
            (intToBinary (i / 2)) + bit

    a + b  |> intToBinary

BinarySum 1 2