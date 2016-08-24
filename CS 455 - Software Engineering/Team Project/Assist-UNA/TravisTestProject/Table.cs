using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

/**************************************************************************************************
 * 
 * Name: Table
 * 
 * ================================================================================================
 * 
 * Description: This class is to serve as the working test model of the final Table Class 
 *              for the ASSIST/UNA project.
 *                            
 *              It is the abstract parent class for the symbol and literal tables.
 *              
 *              The Symbol Table is structured using the Hashtable class from the 
 *              System.Collections directory.
 *              
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/18/2014   THH     Original version.
 * 03/25/2014   THH     Fixed commenting issues to comply with standards.
 * 03/26/2014   THH     Added accessor for the number of items in the table.
 *  
 *************************************************************************************************/

namespace TravisTestProject
{
    abstract class Table
    {
        /* Protected members. */
        protected Hashtable table;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        GetAddress     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The key as a string to be found. 
         * Return:      The address of symbol/literal to be found as string. 
         * Description: This method uses the string passed to it as the key to find the address of
         *              the item saved in the table.
         *              
         *              Needs to be overridden in the child class.
         *              
         *****************************************************************************************/
        abstract public string GetAddress(string key);

        /******************************************************************************************
         * 
         * Name:        GetSize   
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      The number of items. 
         * Description: This method returns the number of items that are stored in the table.
         *              
         *****************************************************************************************/
        abstract public int GetSize();

        /******************************************************************************************
         * 
         * Name:        Hash     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       The symbol/literal and its location as strings. 
         * Return:      N/A 
         * Description: This method takes the symbol/literal as the key and the location as the
         *              value and uses the built in hashing function to store them in the hash table.
         *              
         *              Needs to be overridden in the child class.
         *              
         *****************************************************************************************/
        abstract public void Hash(string key, string location);

        /******************************************************************************************
         * 
         * Name:        isTableFull     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      True if the table is full. 
         * Description: This method checks the number of symbols in the table and returns whether
         *              or not the number is equal to the size of the table.
         *              
         *              Needs to be overridden in the child class.
         *              
         *****************************************************************************************/
        abstract public bool IsTableFull();

        /******************************************************************************************
         * 
         * Name:        PrintTable     
         * 
         * Author(s):   Travis Hunt
         *              
         * Input:       N/A 
         * Return:      N/A 
         * Description: This method prints to the console the contents of the table.
         *              Used for testing only.
         *              
         *              Needs to be overridden in the child class.
         *              
         *****************************************************************************************/
        abstract public void PrintTable();
    }
}
