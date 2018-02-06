# Convert-Plain-Passwords-to-Hash-Salt-with-PBKDF2
This solution will help if you have plain passwords stored in your User Table or a view in a  SQL Server Database, to Encrypt them with a click and store Hash and Salt values.

Storing user passwords is a critical component for any web application. When you store a user’s password, you must ensure that you have it secured in such a way that if your data is compromised, you don’t expose your user’s password.

The "Hashing" function is using PBKDF2 (Password-Based Key Derivation Function 2) algorithm and 51000 number of iterations to secure the password more and create salts.