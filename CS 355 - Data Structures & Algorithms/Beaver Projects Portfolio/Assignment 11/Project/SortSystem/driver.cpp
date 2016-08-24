/*
	Names: Jeffrey Allen and Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 11, Group 3
	Description: This program implements a system design for comparing different sorting algorithms.  The class
				hierarchy allows for "plug-n-play" of sorting algorithm class objects.  The driver program allows
				the user to input data either manually or via file.  The data is sorted and iteration information
				is collected.  The user then has the option to view the iteration information for each algorithm.
    Due Date: November 20, 2012
*/



#include <iostream>
#include <string>
#include <fstream>
#include "SortingSystem.h"

using namespace std;



//-------------------------------------------------------------------


// Bounds which data sets will be automatically printed (size <= MAX_VALUES)
const unsigned int MAX_VALUES = 20; 



//-------------------------------------------------------------------



void loadDataFromFile(string & filename, int & dataSize, AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]);
void loadDataFromUser(string & filename, int & dataSize, AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]);

void readFile(string filename, int * arr, int dataSize);
void saveFile(string filename, AbstractSort * obj, int * arr, int dataSize);

void processData(int * data, int dataSize, string ordering, AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]);
void processData(string filename, int * data, int dataSize, string ordering, AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]);

void makeTables(int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]);

void printArr(int * arr, int size);
void PrintMenu();



//-------------------------------------------------------------------



int main() {

	string filename = "";
	int dataSize = 0;
	char choice;
	

	// Order: 0 = Random, 1 = In Order, 2 = Reverse Order
	// Sorts: 0 = Bubble, 1 = Selection, 2 = Insertion, 3 = Merge, 4 = Quick, 5 = Radix
	// Stats: 0 = Inner Loops, 1 = Outer Loops, 2 = Swaps, 3 = Recursions, 4 = Divisions, 5 = Merges
	// Data Collection    [Order][Sorts][Stats]
	int collectedData10   [  3  ][  6  ][  6  ] = {0};
	int collectedData100  [  3  ][  6  ][  6  ] = {0};
	int collectedData1000 [  3  ][  6  ][  6  ] = {0};
	int collectedData10000[  3  ][  6  ][  6  ] = {0};


	// Sorting objects
	BubbleSort    myBS;
	SelectionSort mySS;
	InsertionSort myIS;
	MergeSort     myMS;
	QuickSort     myQS;
	RadixSort     myRS;

	AbstractSort * mySort[6] = {&myBS, &mySS, &myIS, &myMS, &myQS, &myRS};



	PrintMenu();

	cout << endl << "--> ";
	cin >> choice;


	// Loop until user wants to exit
	while((choice != 'q') && (choice != 'Q')) {
		
		switch(choice) {

			// User wants to load from a file
			case 'f':
			case 'F': 

				cout << endl;
				loadDataFromFile(filename, dataSize, mySort, collectedData10, collectedData100, collectedData1000, collectedData10000);

				break;


			// User wants to input own data manually
			case 'd':
			case 'D':
			
				cout << endl;
				loadDataFromUser(filename, dataSize, mySort, collectedData10, collectedData100, collectedData1000, collectedData10000);

				break;


			// User wants to see collected statistical data
			case 't':
			case 'T':

				makeTables(collectedData10, collectedData100, collectedData1000, collectedData10000);

				break;


			// User wants to see menu
			case 'h':
			case 'H':

				break;


			// User entered invalid choice
			default:
				cout << "ERROR: INVALID CHOICE!" << endl;


		}

		PrintMenu();
		cout << endl << "--> ";
		cin >> choice;

	}

	return 0;

}





/*
  Name: loadDataFromFile
  Writer(s): Michael Beaver
  Purpose: Loads a user's data from a user-specified file.
  Incoming: filename is the name of the file, dataSize is the size of the data set, sorts is an array of
			AbstractSort-derived objects, c10, c100, c1000, c10000 are arrays of collected data
  Outgoing: filename is the name of the input file, dataSize is the size of the data set
  Return: N/A (void)
*/
void loadDataFromFile(string & filename, int & dataSize, AbstractSort * sorts[], 
					int c10[3][6][6], int c100[3][6][6], 
					int c1000[3][6][6], int c10000[3][6][6]) {

	string ordering;
	dataSize = 0;
	fstream file;
	char save = 'y';
	bool badFile = true;



	do {

		cout << "Input file name: ";
		cin >> filename;
		file.open(filename);
		badFile = file.fail();
		file.close();

	} while (badFile);	

	do {

		cout << "Data set size: ";
		cin >> dataSize;

	} while (dataSize <= 0);

	do {

		cout << "Ordering (IO|RAND|RO): ";
		cin >> ordering;

	} while (((ordering != "io")   && (ordering != "IO"))   &&
			 ((ordering != "rand") && (ordering != "RAND")) &&
			 ((ordering != "ro")   && (ordering != "RO")));

	do {

		cout << "Save Ordered Data to File (Y|N): ";
		cin >> save;

	} while ((save != 'y') && (save != 'Y') && (save != 'n') && (save != 'N'));




	// Temporary array to hold data from file
	int * tmpArray = new int [dataSize];

	readFile(filename, tmpArray, dataSize);

	// Perform actual sorting
	if ((save == 'n') || (save == 'N'))
		processData(tmpArray, dataSize, ordering, sorts, c10, c100, c1000, c10000);

	else
		processData(filename, tmpArray, dataSize, ordering, sorts, c10, c100, c1000, c10000);



	delete [] tmpArray;

}






/*
  Name: loadDataFromUser
  Writer(s): Michael Beaver
  Purpose: Loads a user's data from the user -- also give an option to save data to file.
  Incoming: filename is the name of the file, dataSize is the size of the data set, sorts is an array of
			AbstractSort-derived objects, c10, c100, c1000, c10000 are arrays of collected data
  Outgoing: filename is the name of the input file, dataSize is the size of the data set
  Return: N/A (void)
*/
void loadDataFromUser(string & filename, int & dataSize, AbstractSort * sorts[], int c10[3][6][6], 
					 int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]) {


	string ordering = "";
	char save = 'y';
	ofstream customFile;


	do {

		cout << "Data set size: ";
		cin >> dataSize;

	} while (dataSize <= 0);

	do {

		cout << "Ordering (IO|RAND|RO): ";
		cin >> ordering;

	} while (((ordering != "io")   && (ordering != "IO"))   &&
			 ((ordering != "rand") && (ordering != "RAND")) &&
			 ((ordering != "ro")   && (ordering != "RO")));

	do {

		cout << "Save Data to File (Y|N): ";
		cin >> save;

	} while ((save != 'y') && (save != 'Y') && (save != 'n') && (save != 'N'));



	int * tmpArray = new int [dataSize];


	// Get data from user
	for (int i = 0; i < dataSize; i++) {

		cout << "Value\t" << i + 1 << ": ";
		cin >> tmpArray[i];

	}


	// Not saving data to file
	if ((save == 'n') || (save == 'N')) 
		processData(tmpArray, dataSize, ordering, sorts, c10, c100, c1000, c10000);

	// Saving data to file
	else {

		cout << "Output file name (*.*): ";
		cin >> filename;

		customFile.open(filename.c_str());

		for (int i = 0; i < dataSize; i++)
			customFile << tmpArray[i] << endl;

		customFile.close();

		// Actually sort data and save to file
		processData(filename, tmpArray, dataSize, ordering, sorts, c10, c100, c1000, c10000);

	}


	delete [] tmpArray;


}




/*
  Name: readFile
  Writer(s): Jeffrey Allen and Michael Beaver
  Purpose: Reads a list of integral data from a file.
  Incoming: filename is the name of the file, arr is a pointer to the array to hold
			the data, dataSize is the data set size
  Outgoing: arr is a pointer to the array to hold the data
  Return: N/A (void)
*/
void readFile(string filename, int * arr, int dataSize) { 

	int i = 0;
	ifstream inFile;
	
	inFile.open(filename.c_str()); 

	// Read the data from the file
	if (!inFile.fail()) {  

        while ((!inFile.eof()) && (i < dataSize)) {

			inFile >> arr[i];
			i++;

		}

	}

	inFile.close();
}





/*
  Name: saveFile
  Writer(s): Jeffrey Allen and Michael Beaver
  Purpose: Saves statistical information and sorted data to an external file.
  Incoming: filename is the name of the file, obj is a pointer to an AbstractSort object
			arr is a pointer to an array, dataSize is the data set size
  Outgoing: obj is a pointer to an AbstractSort object, arr is a pointer to an array
  Return: N/A (void)
*/
void saveFile(string filename, AbstractSort * obj, int * arr, int dataSize) {

	ofstream outFile;
	string sortType = obj->getSortType();
	int i = 0;

	// Create output file
	outFile.open((sortType + "_" + filename).c_str());

	// Get the sorted array
	arr = obj->getArr();

	// Output stats to file
	outFile << "----------------------------" << endl;
	outFile << "|        STATISTICS        |" << endl;
	outFile << "----------------------------" << endl;
	outFile << "Sorting Algorithm: " << sortType << endl << endl;

	outFile << "Original Data File: " << filename << endl;
	outFile << "Data Set Size: " << dataSize << endl << endl;

	outFile << "Number of Inner Loops: " << obj->getNumInnerLoops() << endl;
	outFile << "Number of Outer Loops: " << obj->getNumOuterLoops() << endl;
	outFile << "Number of Total Loops: " << obj->getNumLoops() << endl << endl;

	outFile << "Number of Swaps: " << obj->getNumSwaps() << endl << endl;

	outFile << "Number of Recursions: " << obj->getNumRecursions() << endl;
	outFile << "Number of Divisions: " << obj->getNumDivisions() << endl;
	outFile << "Number of Merges: " << obj->getNumMerges() << endl << endl;

	outFile << "<end statistics>" << endl << endl << endl;

	outFile << "----------------------------" << endl;
	outFile << "|        SORTED DATA       |" << endl;
	outFile << "----------------------------" << endl << endl;

	// Write sorted data to file
	while (i < dataSize) {

		outFile << arr[i] << endl;
		i++;

	}

	outFile << endl << "<end sorted data>" << endl << endl;

	outFile.close();

}







/*
  Name: processData
  Writer(s): Michael Beaver
  Purpose: Sorts data and stores iteration information.
  Incoming: data is a pointer to an array of data, dataSize is the data set size, sorts is a pointer to
			an array of sorting objects, ordering is how the list is ordered, c10, c100, c1000, c10000 are 
			collections of data from each sorting algorithm
  Outgoing: data is a pointer to an array of data sorts is a pointer to an array of sorting objects
  Return: N/A (void)
*/
void processData(int * data, int dataSize, string ordering, 
				AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], 
				int c1000[3][6][6], int c10000[3][6][6]) {

	system("CLS");

	if (dataSize <= MAX_VALUES) {

		cout << endl << "Original array: " << endl;
		for (int i = 0; i < dataSize; i++)
			cout << data[i] << "\t";

		cout << endl << endl << endl;

	}

	cout <<  "Processing your data. Please wait . . . " << endl << endl;

	int j = 0;

	// Loop through the sorts
	for (int i = 0; i < 6; i++) {

		// Display sort name
		cout << sorts[i]->getSortType() << " . . . ";

		sorts[i]->sort(data, dataSize);

		if ((ordering == "RAND") || (ordering == "rand"))
			j = 0;
		else if ((ordering == "IO") || (ordering == "io")) 
			j = 1;
		else if ((ordering == "RO") || (ordering == "ro"))
			j = 2;

		// Save statistical data for 10 items
		if (dataSize == 10) {

			c10[j][i][0] = sorts[i]->getNumInnerLoops();
			c10[j][i][1] = sorts[i]->getNumOuterLoops();
			c10[j][i][2] = sorts[i]->getNumSwaps();
			c10[j][i][3] = sorts[i]->getNumRecursions();
			c10[j][i][4] = sorts[i]->getNumDivisions();
			c10[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 100 items
		else if (dataSize == 100) {

			c100[j][i][0] = sorts[i]->getNumInnerLoops();
			c100[j][i][1] = sorts[i]->getNumOuterLoops();
			c100[j][i][2] = sorts[i]->getNumSwaps();
			c100[j][i][3] = sorts[i]->getNumRecursions();
			c100[j][i][4] = sorts[i]->getNumDivisions();
			c100[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 1000 items
		else if (dataSize == 1000) {

			c1000[j][i][0] = sorts[i]->getNumInnerLoops();
			c1000[j][i][1] = sorts[i]->getNumOuterLoops();
			c1000[j][i][2] = sorts[i]->getNumSwaps();
			c1000[j][i][3] = sorts[i]->getNumRecursions();
			c1000[j][i][4] = sorts[i]->getNumDivisions();
			c1000[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 10000 items
		else if (dataSize == 10000) {

			c10000[j][i][0] = sorts[i]->getNumInnerLoops();
			c10000[j][i][1] = sorts[i]->getNumOuterLoops();
			c10000[j][i][2] = sorts[i]->getNumSwaps();
			c10000[j][i][3] = sorts[i]->getNumRecursions();
			c10000[j][i][4] = sorts[i]->getNumDivisions();
			c10000[j][i][5] = sorts[i]->getNumMerges();

		}

		//saveFile(filename, sorts[i], tmpArray, dataSize);

		cout << "Done!" << endl << endl;

		if (dataSize <= MAX_VALUES) {

			sorts[i]->print();

			cout << endl << endl;

		}

	}

	cout << endl << "Done!" << endl;

}






/*
  Name: processData
  Writer(s): Michael Beaver
  Purpose: Sorts data, stores iteration information, and saves data to file.
  Incoming: filename is the name of the input file, data is a pointer to an array of data, dataSize is the 
			data set size, sorts is a pointer to an array of sorting objects, ordering is how the list is 
			ordered, c10, c100, c1000, c10000 are collections of data from each sorting algorithm
  Outgoing: data is a pointer to an array of data sorts is a pointer to an array of sorting objects
  Return: N/A (void)
*/
void processData(string filename, int * data, int dataSize, string ordering, 
				AbstractSort * sorts[], int c10[3][6][6], int c100[3][6][6], 
				int c1000[3][6][6], int c10000[3][6][6]) {

	system("CLS");

	if (dataSize <= MAX_VALUES) {

		cout << endl << "Original array: " << endl;
		for (int i = 0; i < dataSize; i++)
			cout << data[i] << "\t";

		cout << endl << endl << endl;

	}

	cout <<  "Processing your data. Please wait . . . " << endl << endl;

	int j = 0;
	int * tmp = 0;

	// Loop through the sorts
	for (int i = 0; i < 6; i++) {

		// Display sort name
		cout << sorts[i]->getSortType() << " . . . ";

		sorts[i]->sort(data, dataSize);

		if ((ordering == "RAND") || (ordering == "rand"))
			j = 0;
		else if ((ordering == "IO") || (ordering == "io")) 
			j = 1;
		else if ((ordering == "RO") || (ordering == "ro"))
			j = 2;

		// Save statistical data for 10 items
		if (dataSize == 10) {

			c10[j][i][0] = sorts[i]->getNumInnerLoops();
			c10[j][i][1] = sorts[i]->getNumOuterLoops();
			c10[j][i][2] = sorts[i]->getNumSwaps();
			c10[j][i][3] = sorts[i]->getNumRecursions();
			c10[j][i][4] = sorts[i]->getNumDivisions();
			c10[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 100 items
		else if (dataSize == 100) {

			c100[j][i][0] = sorts[i]->getNumInnerLoops();
			c100[j][i][1] = sorts[i]->getNumOuterLoops();
			c100[j][i][2] = sorts[i]->getNumSwaps();
			c100[j][i][3] = sorts[i]->getNumRecursions();
			c100[j][i][4] = sorts[i]->getNumDivisions();
			c100[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 1000 items
		else if (dataSize == 1000) {

			c1000[j][i][0] = sorts[i]->getNumInnerLoops();
			c1000[j][i][1] = sorts[i]->getNumOuterLoops();
			c1000[j][i][2] = sorts[i]->getNumSwaps();
			c1000[j][i][3] = sorts[i]->getNumRecursions();
			c1000[j][i][4] = sorts[i]->getNumDivisions();
			c1000[j][i][5] = sorts[i]->getNumMerges();

		}

		// Save statistical data for 10000 items
		else if (dataSize == 10000) {

			c10000[j][i][0] = sorts[i]->getNumInnerLoops();
			c10000[j][i][1] = sorts[i]->getNumOuterLoops();
			c10000[j][i][2] = sorts[i]->getNumSwaps();
			c10000[j][i][3] = sorts[i]->getNumRecursions();
			c10000[j][i][4] = sorts[i]->getNumDivisions();
			c10000[j][i][5] = sorts[i]->getNumMerges();

		}

		tmp = sorts[i]->getArr();

		saveFile(filename, sorts[i], tmp, dataSize);

		cout << "Done!" << endl << endl;

		if (dataSize <= MAX_VALUES) {

			sorts[i]->print();

			cout << endl << endl;

		}

	}

	cout << endl << "Done!" << endl;

}







/*
  Name: makeTables
  Writer(s): Michael Beaver
  Purpose: Prints tables of statistical data for each sorting algorithm.
  Incoming: c10, c100, c1000, c10000 are collections of data from each sorting algorithm
  Outgoing: N/A
  Return: N/A (void)
*/
void makeTables(int c10[3][6][6], int c100[3][6][6], int c1000[3][6][6], int c10000[3][6][6]) {

	system("CLS");

	// c#[order][sort][stats]
	int order = 0;
	int sort = 0;
	int stats = 0;
	int numItems = 0;

	// Loop for every sort (6 sorts)
	while (sort < 6) {

		if (sort == 0) {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                   Bubble Sort                  " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		else if (sort == 1) {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                 Selection Sort                 " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		else if (sort == 2) {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                Insertion Sort                  " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		else if (sort == 3) {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                  Merge Sort                    " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		else if (sort == 4) {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                  Quick Sort                    " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		else  {

			cout << "\t\t------------------------------------------------" << endl;
			cout << "\t\t                  Radix Sort                    " << endl;
			cout << "\t\t------------------------------------------------" << endl;

		}

		// Loop for all the statistics (6 stats => 6 tables)
		while (stats < 6) {

			numItems = 0;
			order = 0;

			if (stats == 0)
				cout << "Inner Loops";
			else if (stats == 1)
				cout << "Outer Loops";
			else if (stats == 2)
				cout << "Swaps\t";
			else if (stats == 3)
				cout << "Recursions";
			else if (stats == 4)
				cout << "Divisions";
			else
				cout << "Merges\t";

			cout << "\t\tRandom\t\tIn Order\tReverse Order" << endl;

			// Loop for the number of items (10, 100, 1000, 10000)
			while (numItems < 4) {

				order = 0;

				if (numItems == 0)
					cout << "  10 items\t\t";
				else if (numItems == 1)
					cout << "  100 items\t\t";
				else if (numItems == 2)
					cout << "  1000 items\t\t";
				else
					cout << "  10000 items\t\t";

				// Output the stats
				while (order < 3) {

					if (numItems == 0)
						cout << c10[order][sort][stats] << "\t\t";
					else if (numItems == 1)
						cout << c100[order][sort][stats] << "\t\t";
					else if (numItems == 2)
						cout << c1000[order][sort][stats] << "\t\t";
					else
						cout << c10000[order][sort][stats] << "\t\t";

					order++;

				}

				cout << endl;

				numItems++;

			}

			stats++;
			cout << endl << endl;

		}

		system("PAUSE");
		system("CLS");
		sort++;
		order = 0;
		stats = 0;

	}

}







/*
  Name: printArr
  Writer(s): Michael Beaver
  Purpose: Prints an array of a certain size.
  Incoming: arr is a pointer to an array and size is the size of the array
  Outgoing: arr is a pointer to an array
  Return: N/A (void)
*/
void printArr(int * arr, int size) {

	for (int i = 0; i < size; i++)
		cout << arr[i] << "\t";

	cout << endl;

}






/*
  Name: PrintMenu
  Writer(s): Jeffrey Allen and Michael Beaver
  Purpose: Prints a menu for the user.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void PrintMenu() {

	cout << endl;
	cout << "\t\t-------------------------------------" << endl;
	cout << "\t\t  Comparison of Sorting Algorithms   " << endl;
	cout << "\t\t-------------------------------------" << endl;
	cout << endl << endl;

	cout << "  H   : Show Command Line Options" << endl << endl;

	cout << "  F   : Read data in from a file" << endl;
	cout << "  D   : Manually input own data" << endl << endl;

	cout << "  T   : Show iteration information" << endl << endl;

    cout << "  Q   : Quit the test program" << endl;
	
}

