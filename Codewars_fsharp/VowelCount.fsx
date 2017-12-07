
let VowelCount1 text =
    text |> Seq.toList
         |> List.fold( fun acc c -> 
                        let i = match c with
                                | 'a' | 'e' | 'i' | 'o' | 'u' -> 1
                                | _ -> 0 
                        acc + i ) 0



let VowelCount2 text = 
    text |> Seq.toList
         |> List.filter( fun c -> 
                                match c with
                                | 'a' | 'e' | 'i' | 'o' | 'u' -> true
                                | _ -> false )
         |> List.length



"steveanbioo" |> VowelCount1 
"steveanbioo" |> VowelCount2



