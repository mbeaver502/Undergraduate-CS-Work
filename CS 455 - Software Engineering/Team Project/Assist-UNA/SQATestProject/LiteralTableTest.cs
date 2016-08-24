using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: LiteralTable
 * 
 * ================================================================================================
 * 
 * Description: This class is to serve as the working test model of the final Literal Table Class 
 *              for the ASSIST/UNA project.
 *                            
 *              It inherits from the abstract Table class.
 *              
 *              The Literal Table is structured using the Hashtable class from the 
 *              System.Collections directory.
 *              
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/18/2014   THH     Original version.
 * 03/25/2014   THH     Fixed commenting issues to comply with standards.
 * 03/25/2014   AAH     Fixed IsLiteral and IsLiteralFull method names.
 *                      Added Main method for testing.
 *  
 *************************************************************************************************/

namespace TravisTestProject
{
    class LiteralTable : Table
    {
        /* Constants. */
        private const int MAX_LITERALS = 50;
        private const int MAX_SIZE = 101;

        /* Private members. */
        private Hashtable literalTable;
        private int numLiterals;


        /* Public methods. */

        public static int Main()
        {
            LiteralTable l = new LiteralTable();

            /* Hash test cases: */
            l.Hash("=F'12'", "000006");
            l.Hash("=F'13'", "00000A");
            l.Hash("=F'14'", "00000C");
            l.Hash("=F'15'", "000020");
            l.Hash("=C' ABC, 123'", "000264");
            l.Hash("=X'53'", "000500");
            /* Check for duplicate entries. */
            /* CAUSES CRASH. 
            l.Hash("=X'53'", "000600");
            */

            l.Hash("=F'40'", "000006");
            l.PrintTable();

            /* GetAddress test cases: */
            Console.WriteLine("=F'12': " + l.GetAddress("=F'12'"));
            Console.WriteLine("=F'13': " + l.GetAddress("=F'13'"));
            Console.WriteLine("=F'14': " + l.GetAddress("=F'14'"));
            Console.WriteLine("=C'ABC, 123': " + l.GetAddress("=C' ABC, 123'"));
            Console.WriteLine("=X'53': " + l.GetAddress("=X'53'"));
            Console.WriteLine("=F'40': " + l.GetAddress("=F'40'"));
            Console.WriteLine();

            /* IsLiteral test cases: */
            if (l.IsLiteral("=F'12'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=F'13'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=F'14'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=C' ABC, 123'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=X'53'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=F'40'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=F'100'"))
                Console.WriteLine("Yes");
            if (l.IsLiteral("=J'12'"))
                Console.WriteLine("Yes");

            Console.ReadKey();
            return 0;
        }

        /******************************************************************************************
         * 
         * Name:        LiteralTable (Constructor)     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: The constructor for LiteralTable. Initializes the literal table as a hash
         *              table and sets the initial number of literals to 0.
         *              
         *****************************************************************************************/
        public LiteralTable()
        {
            literalTable = new Hashtable(MAX_SIZE);
            numLiterals = 0;
        }

        /******************************************************************************************
         * 
         * Name:        GetAddress     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal as a string. 
         * Return:      The address of the literal as a string. 
         * Description: This method uses the literal as a key to find the value, which is the
         *              address of the literal.
         *              
         *****************************************************************************************/
        override public string GetAddress(string key)
        {
            return (string)literalTable[key];
        }

        /******************************************************************************************
         * 
         * Name:        Hash     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal and its location as strings. 
         * Return:      N/A 
         * Description: This method takes the literal as the key and the location as the value and
         *              uses the built in hashing function to store them in the hash table.
         *              
         *****************************************************************************************/
        override public void Hash(string key, string location)
        {
            literalTable.Add(key, location);
            numLiterals++;
        }

        /******************************************************************************************
         * 
         * Name:        isLiteral   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The literal as a string, it is used at the key for the hash table. 
         * Return:      True if the literal is in the table, false if otherwise. 
         * Description: This method checks the table for the existance of a literal.
         *              
         *****************************************************************************************/
        public bool IsLiteral(string key)
        {
            return literalTable.ContainsKey(key);
        }

        /******************************************************************************************
         * 
         * Name:        isLiteralFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if number of literals in table is 50. 
         * Description: This method checks the number of literals in the table and returns whether
         *              or not the number is equal to the max number of literals (50).
         *              
         *****************************************************************************************/
        public bool IsLiteralFull()
        {
            return (literalTable.Count == MAX_LITERALS);
        }

        /******************************************************************************************
         * 
         * Name:        isTableFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if table is full. 
         * Description: This method checks the number of literals in the table and returns whether
         *              or not the number is equal to the max size of the table (101).
         *              
         *****************************************************************************************/
        override public bool IsTableFull()
        {
            return (literalTable.Count == MAX_SIZE);
        }

        /******************************************************************************************
         * 
         * Name:        PrintTable     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: This method prints to the console the contents of the literal table.
         *              Used for testing only.
         *              
         *****************************************************************************************/
        override public void PrintTable()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("|     Literal Table    |");
            Console.WriteLine("| Literal   | Address  |");
            Console.WriteLine("|----------------------|");

            foreach (var key in literalTable.Keys)
                Console.WriteLine(String.Format("| {0} | {1} ", key, literalTable[key]));

            Console.WriteLine("------------------------");
        }
    }
}


