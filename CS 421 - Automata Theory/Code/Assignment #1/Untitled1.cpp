#include <iostream>
#include <string>
#include <unordered_map>
#include <regex>

using namespace std;


string keywords[6] = {"PROGRAM", "VAR", "BEGIN", "END", "END.", "FOR"};





struct KeyHash {
    std::size_t operator()(const string & s) const {
     
        int temp = 0;
        for (unsigned int i = 0; i < s.length(); i++)
            temp += int(s[i]);
        temp *= 2;

        return temp;

    }
};

int main() {

    unordered_map<string, int, KeyHash> abc;
    unordered_map<string, int, KeyHash>::const_iterator got;
  
    cout << "hello world" << endl;

    for (unsigned int i = 0; i < 5; i++)
        abc.insert({keywords[i], i + 1});

    if (abc.count(keywords[0]) == 1)
        cout << "Already hashed" << endl;
/*
    for (unsigned int i = 0; i < 6; i++) {

        got = abc.find(keywords[i]);

        if (got == abc.end())
            cout << "not found: " << keywords[i] << endl;

        else
            cout << got->second << "\t" << got->first << endl;

    }
*/
    for (auto & x: abc)
        cout << x.second << "\t" << x.first << endl;

    return 0;

}
