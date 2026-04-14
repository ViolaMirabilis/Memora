# Memora
A desktop flashcard program designed to help with learning.

<img width="250" alt="1" src="https://github.com/user-attachments/assets/9504faea-adec-4ace-b798-8e85e33c7d60" />
<img width="250" alt="2" src="https://github.com/user-attachments/assets/5f85c0fd-613e-4e30-9567-1b94f1f8cf25" />
<img width="250" alt="3" src="https://github.com/user-attachments/assets/f9cc5c4b-dc7f-4eea-a272-208d4b104fa8" />
<img width="250" alt="4" src="https://github.com/user-attachments/assets/99ab7728-27be-42cc-a7d9-30033de79a11" />
<img width="250" alt="6" src="https://github.com/user-attachments/assets/9cdad322-0793-4531-8879-4c0e7a8b78de" />



## Prerequisities
* .NET 8.0 or higher

## Setting up the project
1) Configure the API endpoint address
By default, the API runs on `localhst:7153` for the `https` endpoints. The address can be changed in `Memora.API/appsettings.json` and `memora.API/Services/API...`

2) Set up startup projects
Both projects need to be running simultaneously (API + Client).
Press on the Gear button next to the green arrow and press `Configure startup projects`

<img width="247" height="142" alt="STARTUP_PROJECTS" src="https://github.com/user-attachments/assets/521ad3f7-bca6-41b5-be62-1b1b520b08e1" />

* Select `Multiple startup projects` and select `Memora.API` and `Memora.WPF` with the `Start` option.


# Current features
* Adding, deleting and modifying flashcards,
* revising flashcards by flipping between the front and the back,
* shuffling all the flashcards,
* test mode, in which the user clicks on one of four ABCD answers,
* grouping flashcards in folders,
* grouping flashcards in flashcard sets,
* replaying currently selected mode (replay revision, replay test mode...)
* sharing the flashcard sets with other users by providing a "sharing code".

# To be added:
- typing mode, where the user types in the answer.
- ... more as requested.
