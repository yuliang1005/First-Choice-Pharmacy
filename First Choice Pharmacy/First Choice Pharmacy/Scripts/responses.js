//source: https://github.com/indently/mscbot
function getBotResponse(input) {
    //rock paper scissors
    if (input == "hi") {
        return "hello";
    } else if (input == "paper") {
        return "scissors";
    } else if (input == "scissors") {
        return "rock";
    }

    // Simple responses
    if (input == "hello") {
        return "Hello there!";
    } else if (input == "goodbye") {
        return "bye!";
    } else {
        return "Try asking something else!";
    }
}