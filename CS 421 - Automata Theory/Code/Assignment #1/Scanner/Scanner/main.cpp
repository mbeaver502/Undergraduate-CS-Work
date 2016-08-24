#include <iostream>
#include <fstream>
#include <string>

using namespace std;

//--------------------------------------------------------------------------

void toUpper(string & str);
void trimSpaces(string & str);
void preProcess(string & str, bool & commentFlag);

//--------------------------------------------------------------------------

int main() {

	ifstream file("prog2.txt");
	string line, buffer;
	int numLines = 0;
	int numErrors = 0;
	bool commentFlag = false;
	
	if (file.is_open()) {

		while (getline(file, line)) {

			toUpper(line);
			buffer = line;
			
			preProcess(buffer, commentFlag);

			if (!commentFlag) {

				cout << numLines + 1 << "\t" << line << endl;

				//

				cout << endl;

				numLines++;

			}

		}

		file.close();

	}

	cout << numLines << " Lines, " << numErrors << " Errors" << endl;

	getchar();
	
	return 0;

}

//--------------------------------------------------------------------------

void toUpper(string & str) {

	int len = str.length();

	for (int i = 0; i < len; i++)
		str[i] = char(toupper(str[i]));

}

//--------------------------------------------------------------------------

void trimSpaces(string & str) {

	string temp;
	int idx = 0;
	int len = str.length();

	while (idx < len) {

		// Replace all sequences of spaces and tabs with a single space
		if (str[idx] == ' ' || str[idx] == '\t') {

			while ((str[idx] == ' ' || str[idx] == '\t') && (idx < len))
				idx++;

			if (idx < len)
				temp += ' ';
		
		} else {

			temp += str[idx];
			idx++;

		}

	}

	idx = 0;
	str = temp;

}

//--------------------------------------------------------------------------

void preProcess(string & str, bool & commentFlag) {

	trimSpaces(str);

}