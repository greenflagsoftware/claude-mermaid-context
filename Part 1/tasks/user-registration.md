# User Registration

## Overview
The interaction will consist of transforming data as it relates to user registration.
This subsystem will need to be responsible for operating on user information including
- Username
- Email
- Password
The subsystem will need to perform the following tasks
1. Validate the input
2. Check password complexity
3. Check in a database if there is another user with the shared username and report an error
4. Save the user registration in the database
5. Send the user an email with a confirmation code that can be later used to confirm the user's email address
6. Report any errors back to the user
7. Write important events and errors to a log file