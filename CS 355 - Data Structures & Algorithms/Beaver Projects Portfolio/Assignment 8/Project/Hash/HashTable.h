/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 8, Homework 6
	Description: This program implements a simple templated HashTable data structure.  For the sake of simplicity,
	             a separate Slot object is not used.  Rather, data is stored in a dynamic array, and a value of
				 -1 indicates that a slot is available to use.  For a more robust solution, a dynamic array of
				 Slot objects should be used.
    Due Date: October 23, 2012

	TO DO: DISALLOW DUPLICATE INPUTS

*/


#ifndef HASHTABLE_H
#define HASHTABLE_H


//-------------------------------------------------------------------------

#define DASHES cout << "-----------------------------------------------" << endl;

//-------------------------------------------------------------------------


template <class T>
class HashTable {

	private:

		// Members
		int originalNumKeys;
		int tableSize;
		T * table;
		bool slotsOpen;
		T initVal;


		// Methods
		bool isPrime(int val) const;

		int NewSlot(T HashVal, int & tryNumber) const;

		bool querySlotsOpen() const;


	public:

		// Constructors and Destructor
		HashTable();
		HashTable(int numKeys, T init);
		HashTable(const HashTable<T> & source);
		~HashTable();


		// Methods and Operators
		HashTable<T>& operator =(const HashTable<T> & source);

		int areOpenSlots();

		void ShowFill() const;
		void ShowContents() const;
		void Print() const;

		int Insert(T key);
		int Remove(T key);
		int Search(T key) const;

		void ClearTable();

		int getOriginalNumKeys() const;


};


//-------------------------------------------------------------------------
// HashTable
//-------------------------------------------------------------------------


/*
  Name: HashTable in scope of HashTable
  Purpose: Default constructor. Sets the tableSize to zero and sets the hash table to NULL.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
template <class T>
HashTable<T>::HashTable() : tableSize(0), table(0), slotsOpen(false), initVal(-1), originalNumKeys(0) { 

	// NO BODY -- SEE INITIALIZATION LIST

}



/*
  Name: HashTable in scope of HashTable
  Purpose: Overloaded Constructor. Sets the tableSize and creates a hash table of appropriate size. All slots
           are initialized to this->initVal (not taken).
  Incoming: numKeys is the number of keys to be stored (basically the minimum size of the hash table)
  Outgoing: N/A
  Return: N/A
*/
template <class T>
HashTable<T>::HashTable(int numKeys, T init) : initVal(init), originalNumKeys(numKeys) {

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

		// Set the tableSize and create the hash table
	
		tableSize = temp;

	}

	else
		tableSize = 1;

	table = new T [tableSize];

	// Initialize all slots to this->initVal (not taken)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;

	slotsOpen = true;
	
}




/*
  Name: HashTable in scope of HashTable
  Purpose: Copy Constructor. Copies hash table values and tableSize from a source HashTable.
  Incoming: source is the original HashTable whose data is to be copied
  Outgoing: source is the original HashTable whose data is to be copied (const modifier)
  Return: N/A
*/
template <class T>
HashTable<T>::HashTable(const HashTable<T> & source) {

	// Copy the source tableSize
	this->tableSize = source.tableSize;


	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {

		this->table = new T [tableSize];

		for (int i = 0; i < this->tableSize; i++) 
			this->table[i] = source.table[i];

	}

	// Otherwise, set this hash table to NULL
	else
		this->table = 0;

	this->slotsOpen = source.slotsOpen;

}




/*
  Name: ~HashTable in scope of HashTable
  Purpose: Destructor. Returns dynamic memory used in allocating the hash table. Sets tableSize and table to NULL.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
template <class T>
HashTable<T>::~HashTable() {

	// Return table memory
	if (table != 0)
		delete [] table;


	// Set back to default -- probably superfluous
	table = 0;
	tableSize = 0;
	slotsOpen = true;

}




/*
  Name: = in scope of HashTable
  Purpose: Assignment Operator. Performs same basic task as Copy Constructor.
  Incoming: source is the original HashTable whose data is to be copied
  Outgoing: source is the original HashTable whose data is to be copied (const modifier)
  Return: Returns a reference to this HashTable object.
*/
template <class T>
HashTable<T> & HashTable<T>::operator =(const HashTable<T> & source) {

	// Check for self-assignment
	if (this == &source)
		return *this;

	// Copy the source tableSize
	this->tableSize = source.tableSize;


	// If there is a source hash table, create a new hash table copy the source values
	if (source.table != 0) {

		if (this->table == 0)
			this->table = new T [tableSize];

		for (int i = 0; i < this->tableSize; i++) 
			this->table[i] = source.table[i];

	}

	// Otherwise, set this hash table to NULL
	else
		this->table = 0;

	this->slotsOpen = source.slotsOpen;

	return *this;

}




/*
  Name: isPrime in scope of HashTable
  Purpose: Checks if a value is prime. Performs basic trial division, for simplicity's sake.
  Incoming: val is the value to be checked for primality.
  Outgoing: N/A
  Return: Returns true if the value is prime and returns false if otherwise.
*/
template <class T>
bool HashTable<T>::isPrime(int val) const {

	// Simple trial division
	int limit = int(sqrt(double(val)));

	for (int i = 2; i <= limit; i++) {

		// Return false if val is a composite number
		if (val % i == 0)
			return false;
			
	}

	return true;

}




/*
  Name: NewSlot in scope of HashTable
  Purpose: Finds the next available slot for insertion. Basically performs continuous linear probing.
  Incoming: HashVal is the original hash location calculated in Insert; tryNumber is the number of slots already
			hit while trying to insert.
  Outgoing: tryNumber is the number of slots hit while trying to find the next available slot
  Return: Returns the slot number of the next available slot. Returns -1 if hash table is full.
*/
template <class T>
int HashTable<T>::NewSlot(T HashVal, int & tryNumber) const  {

	T location = HashVal;

	// Loop through the hash table until an available slot is found
	while (table[location] != this->initVal) {

		// If the table is full, return -1
		if (tryNumber == tableSize)
			return -1;

		location++;
		location %= tableSize;

		tryNumber++;

	}

	return location;

}




/*
  Name: querySlotsOpen in scope of HashTable
  Purpose: Reports the status of slotsOpen -- true => slots available; false => all slots full
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the value stored in slotsOpen
*/
template <class T>
bool HashTable<T>::querySlotsOpen() const {

	return slotsOpen;

}



/*
  Name: areOpenSlots in scope of HashTable
  Purpose: Determines if the hash table is has open slots
  Incoming: N/A
  Outgoing: N/A
  Return: Returns location of first open slot. Returns -1 if no open slots.
*/
template <class T>
int HashTable<T>::areOpenSlots() {

	// Check for open (-1) slots
	for (int i = 0; i < tableSize; i++) {

		if (table[i] == this->initVal) {

			slotsOpen = true;
			return i;

		}

	}

	slotsOpen = false;

	return -1;

}




/*
  Name: ShowFill in scope of HashTable
  Purpose: Displays a table with slot numbers and an X (filled) whenever a slot has data stored.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
template <class T>
void HashTable<T>::ShowFill() const {

	if (tableSize == 0) {

		cout << "(empty)" << endl;
		return;

	}

	DASHES;
	cout << "Location | Taken (X) or Available (blank) " << endl;
	DASHES;

	// Print all slots
	for (int i = 0; i < tableSize; i++) {

		cout << "  " << i << "\t :";

		// If slot is not taken (not -1), then print an X (filled)
		if (table[i] != this->initVal)
			cout << "\tXXXX";

		cout << endl;

	}

	DASHES;

}




/*
  Name: ShowContents in scope of HashTable
  Purpose: Displays a table with slot numbers and data stored at the slots.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
template <class T>
void HashTable<T>::ShowContents() const {
	
	if (tableSize == 0) {

		cout << "(empty)" << endl;
		return;

	}


	DASHES;
	cout << "Location | Contents " << endl;
	DASHES;

	// Print all slots and data
	for (int i = 0; i < tableSize; i++) {

		cout << "  " << i << "\t :";

		// If slot is not taken (not -1), then print its data
		if (table[i] != this->initVal)
			cout << "\t" << table[i];

		cout << endl;

	}

	DASHES;

}




/*
  Name: Print in scope of HashTable
  Purpose: Allows user to call Print instead of ShowContents.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
template <class T>
void HashTable<T>::Print() const {

	this->ShowContents();

}




/*
  Name: Insert in scope of HashTable
  Purpose: Inserts a key into the hash table. 
  Incoming: key is the templated value to insert into the hash table
  Outgoing: N/A
  Return: Returns the number of slots hit while inserting the key.
*/
template <class T>
int HashTable<T>::Insert(T key) {

	// If no available slots, return 0
	if (!slotsOpen) {

		cout << "ERROR: NO SLOTS AVAILABLE FOR " << key << "!" << endl;
		return 0;

	}

	T hashVal;
	int numSlots = 0;

	// Calculate the hash
	hashVal = key % tableSize;
	

	// Try the original position
	if (table[hashVal] == this->initVal) {

		table[hashVal] = key;
		numSlots++;

	}


	// Try again, using linear probing
	else {

		numSlots++;
		int slot = NewSlot(hashVal, numSlots);  // linear probing

		// Found an available slot
		if (slot != this->initVal)
			table[slot] = key;

		else 
			slotsOpen = false;
			
	}

	return numSlots;

}




/*
  Name: Remove in scope of HashTable
  Purpose: Removes a specified key from a slot. A slot's data is replaced with -1 (not taken).
  Incoming: key is the templated value to be removed from the hash table
  Outgoing: N/A
  Return: Returns the number of slots hit while removing the key. Returns 0 if the key is not found.
*/
template <class T>
int HashTable<T>::Remove(T key) {

	// No table, return 0
	if (table == 0)
		return 0;


	T hashVal;
	int numSlots = 0;

	// Compute the hash
	hashVal = key % tableSize;
	
	// Try the original hash location
	if (table[hashVal] == key) {

		table[hashVal] = this->initVal;
		numSlots++;

	}


	// Keep trying until found or not found
	else {

		numSlots++;
		
		T location = hashVal;

		// Loop through the entire hash table, if necessary
		while (table[location] != key) {

			// Not found
			if (numSlots == tableSize)
				return 0;

			else if (table[location] == this->initVal)
				return 0;

			location++;
			location %= tableSize;
			numSlots++;

		}

		table[location] = this->initVal;

	}

	slotsOpen = true;

	return numSlots;

}




/*
  Name: Search in scope of HashTable
  Purpose: Finds a key's slot location in the hash table.
  Incoming: key is the templated value to be found in the hash table
  Outgoing: N/A
  Return: Returns the number of slots hit while finding the key. Returns 0 if the key is not found.
*/
template <class T>
int HashTable<T>::Search(T key) const  {

	// No table, so return 0
	if (table == 0)
		return 0;


	T hashVal;
	int numSlots = 0;

	// Compute the hash
	hashVal = key % tableSize;
	

	// Try the original hash location
	if ((table[hashVal] != this->initVal) && (table[hashVal] == key)) 
		numSlots++;


	// Otherwise, look through the entire hash table, if necessary
	else {

		numSlots++;
		
		T location = hashVal;

		// Loop through the hash table
		while (table[location] != key) {

			// Not found
			if (numSlots == tableSize)
				return 0;

			else if (table[location] == this->initVal)
				return 0;

			location++;
			location %= tableSize;
			numSlots++;

		}

	}

	return numSlots;

}




/*
  Name: ClearTable in scope of HashTable
  Purpose: Replaces all slot data with this->initVal (slot not taken)
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
template <class T>
void HashTable<T>::ClearTable() {

	// Set all values to this->initVal (available)
	for (int i = 0; i < tableSize; i++)
		table[i] = this->initVal;

	slotsOpen = true;
	 
}



/*
  Name: getOriginalNumKeys in scope of HashTable
  Purpose: Returns the original input for numKeys in the data constructor
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the original input for numKeys in the data constructor
*/
template <class T>
int HashTable<T>::getOriginalNumKeys() const {

	return originalNumKeys;

}


#endif