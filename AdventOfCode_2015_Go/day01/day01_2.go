package day01

import (
	"fmt"
	"os"
)

func Executepart2() {
	var fileName string = "./day01/day01.txt"
	var result int = 0
	file, err := os.ReadFile(fileName)
	if err != nil {
		fmt.Printf("Cannot read file %s", fileName)
	}
	fileContent := string(file)
	if fileContent != "" {
		chars := []rune(fileContent)
		for i := 0; i < len(chars); i++ {
			if string(chars[i]) == "(" {
				result = result + 1
			} else {
				result = result - 1
			}
			if result == -1 {
				result = i + 1
				break
			}
		}
	}

	fmt.Println("Result day 01 part 2: %i", result)
}
