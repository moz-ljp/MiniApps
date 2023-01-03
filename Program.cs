using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace p4cs
{
    public class miniApps
    {


        static void Main()
        {
            displayMenu();
        }

        public static void insertGap(int count) //Simple formatting method. Inserts x new lines
        {
            for (int x = 0; x < (count + 1); x++)
            {
                Console.WriteLine(Environment.NewLine);
            }
        }

        /*
         * 
         * Displays the menu using Console.WriteLines
         * Gets and validates user entry
         * Gets user choice via a switch statement
         * Encased in a dowhile loop, the loop is broken when the user selects to quit the application)
         */

        public static void displayMenu() //Displays our menu and allows the user to select an option
        {

            int userChoice = 0; //holds the users selection (after validated) 

            do
            {
                insertGap(2);

                Console.WriteLine("P4CS Mini Applications");
                Console.WriteLine("----------------------");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1) Keep Counting");
                Console.WriteLine("2) Square Root Calculator");
                Console.WriteLine("3) Encrypt Text (Caeser Cipher)");
                Console.WriteLine("4) Decrypt Text (Caeser Cipher)");
                Console.WriteLine("9) Quit");

                Console.WriteLine("Please enter an option: ");

                string entry = Console.ReadLine(); //read into string to validate (using a string because it is the least likely to throw an error when reading
                bool validated = false;
                if (entry != null && entry.Length > 0) //validate against requirements
                    validated = validateIntegerEntry(entry, true);

                if (validated) //if validated, then convert as we know we can do it safely
                    userChoice = Convert.ToInt32(entry);

                switch (userChoice) //using switches for easy expandability later on
                {
                    case 1: //if userChoice is equal to  1
                        keepCounting(); //run the keep counting method and so forth
                        break; //break will break us out of this switch when the keepCounting method has finished
                    case 2:
                        squareRootCalc();
                        break;
                    case 3:
                        caeserCipher(true);
                        break;
                    case 4:
                        caeserCipher(false);
                        break;
                    case 9:
                        break;
                    default: //default is the default occurence if none of our specified cases are used
                        Console.WriteLine("You have selected an invalid option, please try again.");
                        break;

                }
            } while (userChoice != 9); //until the user selects that they want to leave the application


            //Program finishes



        }

        /* Simple method for validating integer entry against requirements
         * We pass the entered string as well as a boolean to determine whether to check if the value needs to be positive.
         * This is because, whilst in square root calculator it needs to be positive, in keep counting, negative values are permitted.
         * 
         */

        public static bool validateIntegerEntry(string entry, bool needsPositive)
        {
            bool passesIsLetterTest = true; //will become false if any letters are found within string
            bool passesIsSymbolPunctuationTest = true; //will become false if any punctuation or symbols are found within string
            bool passesIsWhiteSpaceTest = true; //will become false if there is any whitespace in given string
            bool noOtherExceptions = true; //will become false if the program errors for any other reason
            bool isPositive = false; //will become true if the value is positive or if the value does not need to be positive

            try //first, just try the conversion
            {
                int result = Convert.ToInt32(entry);

                if(needsPositive) //we only need to check this if the conversion works
                {
                    if (Convert.ToInt32(entry) >= 0) //if our value is greater than or equal to 0 (not negative)
                        isPositive = true; //the value is fine to use
                    else
                        Console.WriteLine("Please only enter whole, positive numbers"); //otherwise, report back
                }
                else //if the value doesn't need to be positive, make it true
                {
                    isPositive = true;
                }
                

            }
            catch(ArgumentException) //This is just here in case something isn't set correctly during programming or a very unexpected error occurs.
                {
                Console.WriteLine("Arguments were not passed to validation correctly, please ensure you pass in the following format:");
                Console.WriteLine("(string) user's entry, (boolean) whether the value needs to be positive (i.e greater than 0");
            }
            catch (FormatException) // a format exception is thrown if the inputted value was not expected by the method that we are using for our conversion
            {
                foreach (char x in entry) //so we can iterate over each character in the string, and check them individually to find the culprit
                {
                    if (char.IsLetter(x)) 
                    {
                        Console.WriteLine("Your entry contains a letter, but this entry only accepts integers.");
                        passesIsLetterTest = false;
                        break;
                    }
                    else if (char.IsSymbol(x) || char.IsPunctuation(x))
                    {
                        Console.WriteLine("Your entry contains a symbol/puncutation, but the entry only accepts integers.");
                        passesIsSymbolPunctuationTest = false;
                        break;
                    }
                    else if (char.IsWhiteSpace(x)) //maybe they accidentally hit space on entering etc
                    {
                        Console.WriteLine("Your entry contains whitespace");
                        passesIsWhiteSpaceTest = false;
                        break;
                    }
                }
                Console.WriteLine("Please only enter a positive, whole integer, not any other data types");

            }
            catch (Exception ex) //catch any exceptions and report back, last resort
            {
                Console.WriteLine(ex.Message);
                noOtherExceptions = false;
            }

            return (passesIsLetterTest && passesIsSymbolPunctuationTest && passesIsWhiteSpaceTest && noOtherExceptions && isPositive); //the final check which determines whether the string is acceptable or not

        }

        /*
         *  
         *  ----Test Plan: Keep Counting---
         *  
         *  | INPUT          | EXPECTED                                                                                | ACTUAL       | TEST TYPE       |
         *  ---------------------------------------------------------------------------------------------------------------------------------------------
         *  | (x(+|-)y)      | x becomes (x (+|-) y), correctCounter increments                                        | As expected  | Normal          |
         *  | !(x(+|-)y)     | x becomes (x (+|-) y), correctCounter not incremented, incorrect message displayed.     | As expected  | Normal          |
         *  | string "Hello" | Validation result is displayed, question is marked as incorrect and x becomes (x(+|-)y) | As expected  | Erroneous       |
         *  | char " "       | Whitespace validation returned, question makred as incorrect, x becomes (x(+|-)y)       | As expected  | Erroneous       |
         *  | 2147483648     | Validation message displayed, the given entry is out of bounds for integer datatype     | As expected  | Extreme/Boundary|
         *  | 23.2           | Validaiton message displayed, no punctuation or symbols are allowed in our entry        | As expected  | Erroneous       |
         *  ---------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * 
         *  As shown by the test plan above, the general incorrect entries a user may submit are handled by the validation performed upon the user entry in the method
         *  validateIntegerEntry(). This method checks against some of the criteria for what may be considered a 'normal' erroneous entry but also most other invalid entries
         *  simply by catching the exception itself and reporting back. This should mean that the entry cannot crash our program.
         */

        public static void keepCounting()
        {

            //constants
            Console.WriteLine("-----Keep Counting-----");
            Random rnd = new Random(); //instantiate a new random object to be used
            int correctCount = 0; //stores count of correct questions answered

            bool qOperator = false; //false == +, true == -

            int correctAnswer = 0; //the correct answer to the current question
            int givenAnswer = 0; //the user inputted answer to the current question

            const int lowerRandomBound = 1;
            const int upperRandomBound = 11; //needs to be 11 as it is exclusive
            int x = rnd.Next(lowerRandomBound, upperRandomBound); //declare our variables
            int y = rnd.Next(lowerRandomBound, upperRandomBound);

            const int numberOfQuestions = 10;

            for (int i = 1; i <= numberOfQuestions; i++) //while i is less than numberOfQuestions and increment i each loop
            {

                qOperator = rnd.NextDouble() > 0.5; //generate a random double between 0 and 1 and then use that to determine our boolean value

                if (!qOperator) //if our operator is false, we are adding
                {
                    correctAnswer = x + y;
                    Console.WriteLine(Environment.NewLine + "Question " + i.ToString() + ": " + (x.ToString()) + "+" + (y.ToString()) + "=");
                }
                else
                {
                    correctAnswer = x - y;
                    Console.WriteLine(Environment.NewLine + "Question " + i.ToString() + ": " + (x.ToString()) + "-" + (y.ToString()) + "=");
                }

                string entry = Console.ReadLine(); //read the users entry into a string (the natural data type of Console.ReadLine()
                bool validated = false; //set validated to false until validatation is complete
                if (entry != null) //check if its null first, if it is, no point in validating
                    validated = validateIntegerEntry(entry, false); //validate against integerEntry rules
                if (validated) //if the entry is OK
                    givenAnswer = Convert.ToInt32(entry); //then convert and proceed, knowing the conversion will work.
                else //if validation fails, still give them an incorrect message and allow them to get the question wrong
                {
                    //if i wanted to allow them to try again, I could just decrement i and not update the x and y values
                    Console.WriteLine("INCORRECT: You entered your answer in an incorrect data type.");
                }
                //givenAnswer = Convert.ToInt32(Console.ReadLine()); //convert the given string answer into an int
                if (givenAnswer == correctAnswer) //compare correct answer with entered answer
                {
                    correctCount++; //increment counter of correct answers
                }
                else
                {
                    Console.WriteLine("INCORRECT: ANSWER IS: " + correctAnswer.ToString());
                }
                x = correctAnswer;
                y = rnd.Next(lowerRandomBound, upperRandomBound); //we need to randomise our y operand every time

            }

            Console.WriteLine("You got " + correctCount.ToString() + " right");

        }

        /* --Square Root Calculator Test Plan--
         * 
         * In order to properly test the square root calculator, we will need to also work out some values using a regular, square root operator, done on a calculator
         * Due to the method we are using to calculate square roots (known as the babylonion method of calculating square roots, there is often going to be some 
         * inaccuracy within our results. As long as our result fits within the given precision, we will consider it successful.
         * 
         * Dashes below represent where the program has 'trimmed' 0's from the result as it does not
         * provide any more accuracy to have include them.
         * 
         * Slashes represent instances where that input was never reached, and therefore
         * no value was ever entered.
         * 
         * "Validation Fails" means that validating the number failed, and therefore the code
         * did not run any further. Where validation fails appears, that means my validation
         * code has successfully caught and prevented errors
         * 
         * Due to the nature of the Babylonian method of calculating square roots, and based upon the precision we are calculating to, we have to allow for some
         * inaccuracy in our result; it is not a perfect method but I will consider the test successful if the value is within reasonable bounds
         * 
         * 
         * | Entry       | Chosen Precision | Expected Result     | Actual Result     |  Test Type | Successful | Description
         * -------------------------------------------------------------------------------------------------------------------------------------------------------------
         * |   5         | 1                |   2.2               |  2.2              | Normal     |    Y       | General test.
         * |   8         | 3                |   2.828             |  2.812            | Normal     |    Y       | General test.
         * |   25        | 2                |   5.00              |  4.98             | Normal     |    Y       | Tested as 25 is a perfect square
         * |   1024      | 6                |   32.000000         |  32.0625          | Normal     |    F       | Tested as 1024 is a perfect square + testing precision
         * |   956       | 2                |   30.91             |  30.90            | Normal     |    Y       | General test 
         * |   65986     | 5                |   256.87740         |  256.93973        | Normal     |    F       | Testing precision
         * |   1         | 1                |   1                 |  0.9              | Boundary   |    Y       | Boundary Test (lowest acceptable entry)
         * |   -1        | /                |   Validation fails  |  Validation fails | Erronous   |    Y       | Erroneous test as the program does not accept negative values
         * |   10000     | 2                |   100.00            |  100.02           | Normal     |    Y       | General test
         * |   101.1     | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       | Erroneous test as the program only accepts whole numbers
         * |   100       | 0.1              |   Validation fails  |  Validation fails | Erroneous  |    Y       | Erroneous test as the precision can only be whole numbers between 1-6
         * |  2147483648 | 1                |   Validation fails  |  Validation fails | Extreme    |    Y       | Integer boundary test
         * | -2147483648 | 1                |   Validation fails  |  Validation fails | Extreme    |    Y       | Negative integer boundary test
         * | hello       | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       | Testing integer entry validation
         * | 25          | beans            |   Validation fails  |  Validation fails | Erroneous  |    Y       | Testing integer precision entry validation
         * | !           | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       | Testing integer entry validation
         * | 25          | ?                |   Validation fails  |  Validation fails | Erroneous  |    Y       | Testing precision entry validation
         * -------------------------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * Some of my tests, specifically those with higher precision, are not precise within the given precisions bounds. For example, 1024 should have a precision of 6 decimal places, whereas the actual result only 
         * returns 4 decimal places worth of precision.
         * 
         * The below table is the test table after this error was fixed.
         * * | Entry       | Chosen Precision | Expected Result     | Actual Result     |  Test Type | Successful |
         * ------------------------------------------------------------------------------------------------------
         * |   5         | 1                |   2.2               |  2.3              | Normal     |    Y       |
         * |   8         | 3                |   2.828             |  2.829            | Normal     |    Y       |
         * |   25        | 2                |   5.00              |  5.00             | Normal     |    Y       |
         * |   1024      | 6                |   32.000000         |  32.000001        | Normal     |    Y       |
         * |   956       | 2                |   30.91             |  30.92            | Normal     |    Y       |
         * |   65986     | 5                |   256.87740         |  256.87740        | Normal     |    Y       |
         * |   1         | 1                |   1                 |  0.9              | Boundary   |    Y       |
         * |   -1        | /                |   Validation fails  |  Validation fails | Erronous   |    Y       |
         * |   10000     | 2                |   100.00            |  99.99            | Normal     |    Y       |
         * |   101.1     | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * |   100       | 0.1              |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * |  2147483648 | 1                |   Validation fails  |  Validation fails | Extreme    |    Y       |
         * | -2147483648 | 1                |   Validation fails  |  Validation fails | Extreme    |    Y       |
         * | hello       | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * | 25          | beans            |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * | !           | /                |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * | 25          | ?                |   Validation fails  |  Validation fails | Erroneous  |    Y       |
         * ------------------------------------------------------------------------------------------------------
         * 
         * Fixing this error not only has made the previously unsuccessful tests become successful but have also increased the accuracy of some of my values.
         * Furthermore, the values now also are always rounded to the relevant decimal places as requested by the user.
         * 
         */

        public static void squareRootCalc()
        {
            Console.WriteLine("-----Square Root Calculator-----");

            List<decimal> precisions = new List<decimal> { 0.1m, 0.01m, 0.001m, 0.0001m, 0.00001m, 0.000001m }; //our decimal precisions

            bool entryValidated = false; //holds validation results
            bool decimalPlacesValidated = false;

            decimal numToSquareRoot = 0; //this will hold the desired value to square root
            int decimalPlaces = 0; //this will be the index of the desired precisions in the list
            decimal precision = precisions[decimalPlaces]; //get the precision from the list using the int as our index

            do //if the validation fails, just make them enter both numbers again. This will allow them to try a new set of data if the data they are trying to use is incorrect, rather than having to exit back to the menu
            {
                Console.WriteLine("Please enter a positive number: ");
                string userEntry = Console.ReadLine(); //get user input into a string
                if (validateIntegerEntry(userEntry, true)) //check it against integer validation
                {
                    numToSquareRoot = Convert.ToDecimal(userEntry); //we know it is fine to convert so convert it into a decimal (integer validation will still check for the same requirements as decimal validation)
                    entryValidated = true; 
                    Console.WriteLine("How many decimal places do you want the solution calculated to: (between 1 and 6) ");
                    userEntry = Console.ReadLine(); //get users desired precision

                    if (validateIntegerEntry(userEntry, true))
                    {
                        decimalPlaces = Convert.ToInt32(userEntry); //check that it is within our range
                        if (decimalPlaces > 0 && decimalPlaces < 7)
                        {
                            decimalPlacesValidated = true;
                            precision = precisions[decimalPlaces-1];
                        }

                    }
                }
 
                

            } while (!(entryValidated && decimalPlacesValidated));

            //Console.WriteLine("Entry validated: " + numToSquareRoot);

            decimal lowerBound = 0m; //set our bounds, 0 being lower
            decimal upperBound = Convert.ToDecimal(numToSquareRoot); // the number itself being the upper bound
            //Console.WriteLine("Upper bound: " + upperBound + " Lower bound: " + lowerBound);
            //Console.WriteLine("Precision: " + precision);
            decimal average = 0m; //variables to hold our average and average^2
            decimal averageSquared = 0m;

            while((upperBound - lowerBound) > precision) //while the value is greater than our desired precision, iterate
            {
                average = ((upperBound + lowerBound) / 2); //get the average
                //Console.WriteLine("Average: " + average);
                averageSquared = average*average; //square it
                //Console.WriteLine("Average^2: " + averageSquared);
                //Console.WriteLine("Upper Bound: " + upperBound);
                //Console.WriteLine("Lower bound: " + lowerBound);

                if (averageSquared > numToSquareRoot) //assign to upper or lower bound accordingly
                {
                    upperBound = average;
                    //Console.WriteLine("Updating upper");
                }
                    
                else
                {
                    lowerBound = average;
                    //Console.WriteLine("Updating lower");
                }
                    

            }

            //Console.WriteLine("Final average:" + average);

            decimal result = Math.Round(average, decimalPlaces); //once finished, format the result accordingly.
            //Console.WriteLine(Math.Round(result, 6));

            Console.WriteLine("Result: " + result);


        }


        public static void caeserCipher(bool encrypting) //Encrypting = true means we are encrypting, false means decrypting
        {
            Console.WriteLine("-----Encrypt / Decrypt Text-----"); //just retrieving the users entry here. 

            bool entryValidated = false;
            bool shiftEntryValidated = false;

            string toEncrypt = "";
            int shiftKey = 0;

            do
            {
                if (encrypting) //output based on encrypting boolean to make relevant for current situation
                    Console.WriteLine("Enter text to encrypt: "); 
                else
                    Console.WriteLine("Enter text to decrypt: ");
                toEncrypt = Console.ReadLine();
                toEncrypt = toEncrypt.ToUpper();
                entryValidated = validateStringEntry(toEncrypt);
                Console.WriteLine("Enter shift: ");
                string shiftKeyEntry = Console.ReadLine();
                bool validated = validateIntegerEntry(shiftKeyEntry,false); //we can use our normal integer validation to ensure it is in fact an integer.
                if (validated)
                    shiftKey = Convert.ToInt32(shiftKeyEntry); //then we can check if its within the set bounds for the caeser cipher
                if (shiftKey > 0 && shiftKey < 37)
                    shiftEntryValidated = true;
                else
                    Console.WriteLine("You have entered an invalid shift key, it must be within the range of 0 and 36 (inclusive)");


            } while (!(entryValidated && shiftEntryValidated));


            string result = "";



            if (encrypting)
                result = decipherOrEncipher(toEncrypt.ToCharArray(), shiftKey, true); //pass true to the algorithm if encrypting
            else
                result = decipherOrEncipher(toEncrypt.ToCharArray(), shiftKey, false); //otherwise false
            Console.WriteLine("Encoded/Decoded string is: '" + result+ "'");


        }

        public static char[] getAlphabet() //centralise location of our alphabet to ensure that changes apply to all methods (if we needed to access anywhere else in the program)
        {
            char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0','1',
        '2', '3', '4', '5', '6', '7', '8', '9', ' '}; //the set of chars we are required to shift against, can be added to as needed (also change the value we mod by)
            return (alphabet);
        }

        public static bool validateStringEntry(string entry) //simple validation to check that the entered characters are within our char array for the caeser cipher
        {
            bool validated = true; //true until issue arises

            char[] alphabet = getAlphabet(); //get the acceptable alphabet for the caeser cipher
            foreach (char x in entry) //iterate over each character
            {
                if (!alphabet.Contains(x))  //if the character is not in the alphabet
                {
                    validated = false; //set to false
                    Console.WriteLine("Your entry contains a character that is not in our alphabet");
                }

            }
            return validated;
        }


        /* We need to shift the index of the characters in the alphabet by the given key
         * To deal with overflow, we can use modulus to 'wrap around' the array
         * The alogorith is (key + index of current letter)%lengthofchararray
         * Or in our case:
         * the algorithm is (key + index of current letter)%36
         * This will then allow us to wrap around the alphabet where needed to continue to encrypt our string
         * We will then get an index given to us in return which is the index of our new character, shifted by the given key
         * We can then add the new character at the index to our encrypted string
         * Reversing this is as easy as reversing the algorithm. Instead of adding the current index to the shift, we subtract the shift from the current index
         * 
         * My Caeser Cipher solution was influenced by a solution I found during research that made the most sense to me and that I could understand enough to be able to build my own solution (which is more concise and has less repeating code)
         * https://gist.github.com/jrs-srj/8336f736fce654ae0ceaf7a0220bd1bf
         * I have written and adapted the solution here to my own functional version, compressing the two methods into one and holding the alphabet
         * in one central location for easy updating and consistency.
         * I have also replaced C#'s in built modulus function with an adapted versoin of one I found during my research as C#'s modulus function does not deal with
         * negative values as the regular modulus function would normally do.
         * 
         * My caeser cipher essentially splits the given string into an array of characters and then iterates over said characters, shifting the index to the new index
         * provided by the function mod(givenkey + currentindex) to give the new character. This character is then added to a new array of characters which is 
         * converted into a string at the end of the function.
         * 
         *      ------Test Plan (Encrypting)------
         *      
         *      |String to encrypt|Key |Expected Result   |Actual Result    | Completes? |Type            |
         *      |-----------------------------------------------------------------------------------------|
         *      |Hello            |3   |KHOOR             |KHOOR            | Y          |Normal          |
         *      |computer science |12  |O1Y276Q4L5OUQZOQ  |O1Y276Q4L5OUQZOQ | Y          |Normal          |
         *      |numbers 123456789|5   |SZRGJWXE6789 ABCD |SZRGJWXE6789 ABCD| Y          |Normal          |
         *      |ABCDEFGHIJKLMNOPQ|30  |56789 ABCDEFGHIJK |56789 ABCDEFGHIJK| Y          |Normal          |
         *      |#12#             |2   |Flagged entry     |Flagged entry    | Y          |Erroneous       |
         *      |'!()//           |16  |Flagged Entry     |Flagged entry    | Y          |Erroneous       |
         *      |A 9              |36  |A 9               |A 9              | Y          |Extreme/Boundary|
         *      |A 9              |37  |Flagged entry     |Flagged entry    | Y          |Extreme/Boundary|
         *      -------------------------------------------------------------------------------------------
         *      This test plan covers expected values, as well as some erroneous values that we expect to be caught by the validation I have implemented.
         *      I have also included extreme/boundary tests to ensure that entries that are directly at the start or end of our valid entry range still function as expected.
         *      
         *      ------Test Plan (Decrypting)------
         *      |String to encrypt|Key |Expected Result   |Actual Result    | Completes? |Type            |
         *      |-----------------------------------------------------------------------------------------|
         *      |KHOOR            |3   |HELLO             |HELLO            | Y          |Normal          |
         *      |O1Y276Q4L5OUQZOQ |12  |COMPUTER SCIENCE  |COMPUTER SCIENCE | Y          |Normal            
         *      
         */

        public static string decipherOrEncipher(char[] stringToEncrypt, int key, bool encipher) //if encipher is true, we are encrypting, if false - decrypting
        {
            char[] alphabet = getAlphabet();

            int alphabetLength = alphabet.Length;

            int length = stringToEncrypt.Length; //we need the length to determine how large the instantiated char array beneath should be
            char[] encryptedChars = new char[length]; //we need to instantiate an empty array of characters to add to as we iterate over the list of unencrypted characters 

            for (int x = 0; x < length; x++) //using this so we can use i for the position of our element in the list
            {
                var currentLetter = stringToEncrypt[x]; //get the current letter in our given set of chars to encrypt
                int currentIndex = Array.IndexOf(alphabet, currentLetter); //and find the index of said char
                if (encipher)
                {
                    encryptedChars[x] = alphabet[mod((key + currentIndex), alphabetLength)]; //use this algorithm, which either shifts by given key, or wraps around the array where needed using modulus to give us our remainder
                    //Console.WriteLine(alphabet[currentIndex]);
                    //Console.WriteLine(encryptedChars[x]);
                }

                else
                {
                    //Console.WriteLine((currentIndex - key) % 36);
                    //Console.WriteLine(-1 % 36);
                    //Console.WriteLine("Encrypted character: " + alphabet[currentIndex]);
                    //Console.WriteLine("Current index " + currentIndex);
                    //Console.WriteLine("Amount to shift by: " + ((currentIndex - key) % 36));
                    encryptedChars[x] = alphabet[mod((currentIndex - key), alphabetLength)]; //modified version of earlier algorithm, subtracts our currentIndex by the given key to revert the encryption, using Math.Abs to stop it from being a negative
                    //Console.WriteLine(("Decrypted character " + encryptedChars[x] + Environment.NewLine));
                }

            }

            string encryptedString = string.Join("", encryptedChars); //turn our chars into a string (https://www.dotnetperls.com/string-join)
            return encryptedString; //return to be displayed
        }

        public static int mod(int toMod, int modValue) //(CITE) stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain  as the c# modulus operator cannot properly deal with negatives
        { //through looking through other peoples solutions, i have managed to come up with this alogorithm which can correctly perform mod on negatives

            int preMod = (toMod % modValue + modValue);
            int postMod = preMod % modValue;

            return (postMod);
        }

    }
}
