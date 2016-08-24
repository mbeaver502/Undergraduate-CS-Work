/*
Michael Beaver
CS 421 - Automata Theory & Compiler Construction
Assignment #1 - MINI-P Scanner
20 March 2015
Program Description: 
	This program implements a Scanner for a MINI-P compiler. This program emulates
	the basic function of a Parser as it would continuously prompt the Scanner to
	scan tokens in the buffer. This implementation assumes that a source file is
	being read line-by-line. That is, the lookahead buffer is assumed to be a 
	single line rather than a continuous stream of bytes. This could easily be
	adjusted if necessary.
*/

#include <math.h>
#include <string.h>
#include "SymbolTable.h"

//-------------------------------------------------------------------------

/*
Function: SymbolTable
Purpose: Default constructor.
Parameters: N/A
Return: N/A
*/
SymbolTable::SymbolTable() {

    tableSize = 0;
    table = 0;
    slotsOpen = false;
    originalNumKeys = 0;

	initVal.key = -1;
	initVal.code = -1;
	
}

//-------------------------------------------------------------------------

/*
Function: SymbolTable
Purpose: Overloaded constructor. Specifies the number of keys expected and creates
		 the internal table to accomodate those keys.
Parameters: numKeys is the number of expected keys
Return: N/A
*/
SymbolTable::SymbolTable(const int numKeys) : originalNumKeys(numKeys) {

	// In case of invalid value for numKeys
	if (numKeys < 0) {
		table = 0;
		tableSize = 0;
		slotsOpen = false;
		return;
	}

	// For any values of numKeys > 0
	if (numKeys != 0) {

		// Double the number of keys
		int temp = (numKeys * 2);

		// Then find a prime number (hopefully close by) to set as tableSize
		while (!isPrime(temp))
			temp++;

		tableSize = temp;

	}

	else
		tableSize = 1;

	table = new Record[tableSize];

	this->initVal.key = -1;
	this->initVal.code = -1;

	// Initialize all slots to this->initVal (not taken)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;

	slotsOpen = true;

}

//-------------------------------------------------------------------------

/*
Function: SymbolTable
Purpose: Copy constructor.
Parameters: source is a reference to the source Symbol Table (i.e., to be copied)
Return: N/A
*/
SymbolTable::SymbolTable(const SymbolTable & source) {

	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {
		this->tableSize = source.tableSize;
		this->slotsOpen = source.slotsOpen;
		this->table = new Record[tableSize];
		
		for (int i = 0; i < this->tableSize; i++)
			this->table[i] = source.table[i];
	}

	// Otherwise, set this hash table to NULL
	else {
		this->tableSize = 0;
		this->slotsOpen = false;
		this->table = 0;
	}

}

//-------------------------------------------------------------------------

/*
Function: ~SymbolTable
Purpose: Destructor. Returns allocated memory.
Parameters: N/A
Return: N/A
*/
SymbolTable::~SymbolTable() {

	// Return table memory
	if (table != 0)
		delete[] table;


	// Set back to default -- probably superfluous
	table = 0;
	tableSize = 0;
	slotsOpen = false;

}

//-------------------------------------------------------------------------

/*
Function: operator =
Purpose: Assignment operator. Overloaded to allow assignment of Symbol Tables.
Parameters: source is a reference to the Symbol Table from which to assign
Return: Reference to a new Symbol Table
*/
SymbolTable & SymbolTable::operator =(const SymbolTable & source) {

	// Check for self-assignment
	if (this == &source)
		return *this;

	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {
		this->tableSize = source.tableSize;
		this->slotsOpen = source.slotsOpen;

		if (this->table == 0)
			this->table = new Record[tableSize];

		for (int i = 0; i < this->tableSize; i++)
			this->table[i] = source.table[i];
	}

	// Otherwise, set this hash table to NULL
	else {
		this->tableSize = 0;
		this->slotsOpen = false;
		this->table = 0;
	}

	return *this;

}

//-------------------------------------------------------------------------

/*
Function: isPrime
Purpose: This function determines if an integer is prime.
Parameters: val is the integer to test
Return: true if prime, false otherwise
*/
bool SymbolTable::isPrime(const int val) const {

	// Simple trial division
	int limit = int(sqrt(double(val)));

	for (int i = 2; i <= limit; i++) {
		if (val % i == 0)
			return false;
	}

	return true;

}

//-------------------------------------------------------------------------

/*
Function: NewSlot
Purpose: This function tries to find a new (i.e., unoccupied) slot in the table.
Parameters: HashVal is an unsigned integer that serves as the location mapping,
			tryNumber is a reference to the number of times the table has tried
			to find a new slot
Return: The location of an available slot, -1 on failure
*/
int SymbolTable::NewSlot(const unsigned int HashVal, int & tryNumber) const  {

	unsigned int location = HashVal;

	// Loop through the hash table until an available slot is found
	while ((table[location]).key != (this->initVal).key) {
		if (tryNumber == tableSize)
			return -1;

		location++;
		location %= tableSize;
		tryNumber++;
	}

	return location;

}

//-------------------------------------------------------------------------

/*
Function: querySlotsOpen
Purpose: This function returns the status of the slots in the table (i.e., slots
		 available or all full)
Parameters: N/A
Return: true if slots available, false otherwise
*/
bool SymbolTable::querySlotsOpen() const { return slotsOpen; }


//-------------------------------------------------------------------------

/*
Function: doHash
Purpose: This function calculates the hash value of a C-string (in a needlessly
		 complex yet fun way) 
Parameters: name is the C-string to hash
Return: Unsigned hash value
See: // http://www.eternallyconfuzzled.com/tuts/algorithms/jsw_tut_hashing.aspx
*/
unsigned int SymbolTable::doHash(const char name[]) const {

	unsigned int hash = 0;
	unsigned int len = strlen(name);

	for (unsigned int i = 0; i < len; i++) {
		hash += name[i];
		hash += (hash << 10);
		hash ^= (hash >> 6);
	}

	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);

	return hash;

}

//-------------------------------------------------------------------------

/*
Function: areOpenSlots
Purpose: This function tries to find an open slot within the table.
Parameters: N/A
Return: Slot number of the first available slot, -1 on failure
*/
int SymbolTable::areOpenSlots() {

	// Check for open (i.e., initVal => available) slots
	for (int i = 0; i < tableSize; i++) {
		if (table[i].code == (this->initVal).code) {
			slotsOpen = true;
			return i;
		}
	}

	slotsOpen = false;

	return -1;

}

//-------------------------------------------------------------------------

/*
Function: Insert
Purpose: This function inserts a Record into the table.
Parameters: key is the Record to be inserted
Return: Number of slots tried during insertion, -1 on failure
*/
int SymbolTable::Insert(const Record key) {

	unsigned int hashVal;
	int numSlots = 0;

	if (!slotsOpen) 
		return -1;

	// Calculate the hash
	hashVal = doHash(key.name) % tableSize;

	
	if (((table[hashVal]).key == (this->initVal).key) &&
		(strcmp((table[hashVal]).name, (this->initVal).name) == 0) &&
		((table[hashVal]).code == (this->initVal).code)) 
	{
		table[hashVal] = key;
		(table[hashVal]).key = int(hashVal);
		numSlots++;
	}
	

	// Try again, using linear probing
	else {
		numSlots++;
		int slot = NewSlot(hashVal, numSlots);

		// Found an available slot
		if ((table[slot]).key == (this->initVal).key) {
			table[slot] = key;
			(table[slot]).key = int(slot);
			
			Record x;
			x = table[slot];
			x = x;
		}

		else
			slotsOpen = false;
	}

	return numSlots;

}

//-------------------------------------------------------------------------

/*
Function: Remove
Purpose: This function attempts to remove a C-string's associated Record from
		 the table.
Parameters: name is the C-string whose Record is to be removed
Return: true on success, false on failure
*/
bool SymbolTable::Remove(const char name[]) {
	
	unsigned int hashVal;
	int numSlots = 0;

	// No table
	if (table == 0)
		return false;

	// Compute the hash
	hashVal = doHash(name) % tableSize;

	// Try the original hash location
	if (strcmp((table[hashVal]).name, name) == 0) {
		table[hashVal] = this->initVal;
		numSlots++;
		slotsOpen = true;
	}

	// Keep trying until found or not found
	else {
		numSlots++;
		unsigned int location = hashVal;

		// Loop through the entire hash table, if necessary
		while (strcmp((table[location]).name, name) != 0) {
			if (numSlots == tableSize)
				return false;
			else if ((table[location]).key == (this->initVal).key)
				return false;

			location++;
			location %= tableSize;
			numSlots++;
		}

		table[location] = this->initVal;
	}

	return true;

}

//-------------------------------------------------------------------------

/*
Function: Search
Purpose: This function attempts to locate a C-string's associated Record within
		 the table.
Parameters: name is the C-string whose Record is to be found
Return: Slot of Record, -1 on failure
*/
int SymbolTable::Search(const char name[]) const  {
	
	unsigned int hashVal;
	int numSlots = 0;
	int location;

	// No table
	if (table == 0)
		return -1;

	// Compute the hash
	hashVal = doHash(name) % tableSize;

	// Try the original hash location
	if (((table[hashVal]).key != (this->initVal).key) && 
		(strcmp((table[hashVal]).name, name) == 0)) 
	{
		location = (table[hashVal]).key;
		numSlots++;
	}
		
	// Otherwise, look through the entire hash table, if necessary
	else {
		numSlots++;
		location = int(hashVal);

		// Loop through the hash table
		while (strcmp((table[location]).name, name) != 0) {
			if (numSlots == tableSize)
				return -1;
			else if ((table[location]).key == (this->initVal).key)
				return -1;

			location++;
			location %= tableSize;
			numSlots++;
		}
	}

	return location;

}

//-------------------------------------------------------------------------

/*
Function: ClearTable
Purpose: This function completely empties the internal table.
Parameters: N/A
Return: N/A (void)
*/
void SymbolTable::ClearTable() {

	// Set all values to this->initVal (available)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;
	slotsOpen = true;

}

//-------------------------------------------------------------------------

/*
Function: getOriginalNumKeys
Purpose: This function returns the original number of keys specified in the
		 overloaded constructor.
Parameters: N/A
Return: The original number of keys specified
*/
int SymbolTable::getOriginalNumKeys() const { return originalNumKeys; }

//-------------------------------------------------------------------------

/*
Function: getTableSize
Purpose: This function returns the size of the internal table.
Parameters: N/A
Return: The size of the internal table
*/
int SymbolTable::getTableSize() const {	return this->tableSize; }

//-------------------------------------------------------------------------

/*
Function: getRecordCode
Purpose: This function returns the code stored in a Record.
Parameters: name is the C-string whose Record's code is to be returned
Return: Code stored within the Record
*/
int SymbolTable::getRecordCode(const char name[]) const {

	int location = this->Search(name);

	if (location < 0)
		return (this->initVal).code;
	else
		return (this->table[location]).code;

}

//-------------------------------------------------------------------------

/*
Function: setRecordCode
Purpose: This function sets the code within a Record in the table.
Parameters: name is the C-string whose Record is to be updated, code is the new code
Return: true on success, false on failure
*/
bool SymbolTable::setRecordCode(const char name[], const int code) {

	int location = this->Search(name);

	if (location < 0)
		return false;

	(this->table[location]).code = code;

	return true;

}

//-------------------------------------------------------------------------

/*
Function: operator []
Purpose: This function is used to access the name field of a Record in the table.
Parameters: key is the location of the Record within the table
Return: Character string containing the Record's name field contents, NULL if
		there is no internal table
*/
char * SymbolTable::operator [](const int key) const {

	if (table == 0)
		return NULL;

	return (this->table[key]).name;

}
