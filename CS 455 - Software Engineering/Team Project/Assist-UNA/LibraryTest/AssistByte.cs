using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: AssistByte
 * 
 * ================================================================================================
 * 
 * Description: This class represents the a byte that can be "locked" (i.e., declared constant).            
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/29/2014    JMB     Created class, members, and methods.
 * 04/03/2014    JMB     Corrected variables to conform to standards.
 * 04/05/2014    JMB     Reordered methods to conform to standards.
 *
 *************************************************************************************************/

namespace LibraryTest
{
    class AssistByte
    {
        /* Private members. */
        bool locked;
        SByte myByte;


        /******************************************************************************************
         * 
         * Name:        AssistByte 
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A 
         * Return:      N/A   
         * Description: This default constructor initializes the AssistByte to a default state.
         *              
         *****************************************************************************************/
        public AssistByte()
        {
            myByte = new SByte();
            locked = false;
        }

        /******************************************************************************************
         * 
         * Name:        AssistByte 
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       The lockFlag is boolean.
         * Return:      N/A  
         * Description: This overloaded constructor initializes the AssistByte to be locked or 
         *              unlocked, as specified by "lockFlag."
         *              
         *****************************************************************************************/
        public AssistByte(bool lockFlag)
        {
            myByte = new SByte();
            locked = lockFlag;
        }

        /******************************************************************************************
         * 
         * Name:        IsLocked
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A   
         * Return:      The result is boolean.  
         * Description: This method returns whether or not the byte is locked. If the byte is
         *              locked, then the returned result is true. Otherwise, the returned result
         *              is false.
         *              
         *****************************************************************************************/
        public bool IsLocked()
        {
            return locked;
        }

        /******************************************************************************************
         * 
         * Name:        SetValue
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       The val is an integer. 
         * Return:      The result is boolean.
         * Description: This method will attempt to change the value of the byte to the value
         *              specified by "val." The value will not change if the byte is locked or if
         *              an error occurs. If an error occurs, the returned result will be false.
         *              Otherwise, the returned result will be true.
         *              
         *****************************************************************************************/
        public bool SetValue(int val)
        {
            bool result = false;
            int oldVal;
            string temp;

            /* Save the original value. */
            oldVal = this.GetInt();

            try
            {
                /* Do not overwrite locked bytes. */
                if (locked)
                    result = false;

                /* Attempt to overwrite byte. */
                else
                {
                    temp = val.ToString();
                    result = SByte.TryParse(temp, out myByte);
                }
            }

            /* Exceptions result in false return. */
            catch (ArgumentNullException)
            {
                result = false;
            }

            catch (FormatException)
            {
                result = false;
            }

            catch (OverflowException)
            {
                result = false;
            }

            /* If unsuccessful, replace the original value. */
            if (result == false)
            {
                temp = oldVal.ToString();
                myByte = SByte.Parse(temp);
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetValue
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       The val is a string. 
         * Return:      The result is boolean.
         * Description: This method will attempt to change the value of the byte to the value
         *              specified by "val." The value stored in "val" should be the signed value
         *              converted from hexadecimal. The value will not change if the byte is locked 
         *              or if an error occurs. If an error occurs, the returned result will be false.
         *              Otherwise, the returned result will be true.
         *              
         *****************************************************************************************/
        public bool SetValue(string val)
        {
            bool result = false;
            int oldVal;
            string temp;

            /* Save the original value. */
            oldVal = this.GetInt();

            try
            {
                /* Do not overwrite locked bytes. */
                if (locked)
                    result = false;

                /* Attempt to overwrite byte. */
                else
                {
                    //temp = Convert.ToString(Convert.ToSByte(val, 16));
                    result = SByte.TryParse(val, out myByte);
                }
            }

            /* Exceptions result in false return. */
            catch (ArgumentNullException)
            {
                result = false;
            }

            catch (FormatException)
            {
                result = false;
            }

            catch (OverflowException)
            {
                result = false;
            }

            /* If unsuccessful, replace the original value. */
            if (result == false)
            {
                temp = oldVal.ToString();
                myByte = SByte.Parse(temp);
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        GetInt
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A 
         * Return:      The result is an integer.  
         * Description: This method returns the integer value of the byte.
         *              
         *****************************************************************************************/
        public int GetInt()
        {
            return myByte;
        }

        /******************************************************************************************
         * 
         * Name:        GetHex
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A 
         * Return:      The result is a string (hexadecimal).
         * Description: This method returns the hexadecimal (two characters) representation of the
         *              byte's value.
         *              
         *****************************************************************************************/
        public string GetHex()
        {
            return myByte.ToString("X").PadLeft(2, '0');
        }

        /******************************************************************************************
         * 
         * Name:        ToggleLock
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A 
         * Return:      N/A  
         * Description: This method toggles the byte lock. If the byte is locked, then it is 
         *              unlocked. If the byte is unlocked, then it is locked.
         *              
         *****************************************************************************************/
        public void ToggleLock()
        {
            locked = !locked;
        }

    }

}
