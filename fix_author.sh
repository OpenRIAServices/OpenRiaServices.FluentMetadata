#!/bin/sh

# Merijn de Jonge <merijn.de.jonge@sorama.eu>
# MdeJ <none@none.com>
#  

git filter-branch -f --env-filter '

OLD_NAME="MdeJ_cp"
OLD_EMAIL="none@none.com"
OLD_EMAIL="merijn.de.jonge@sorama.eu"
CORRECT_NAME="Merijn de Jonge"
CORRECT_EMAIL="merijn.de.jonge@gmail.com"

if [ "$GIT_COMMITTER_NAME" = "$OLD_NAME" ]
then
    export GIT_COMMITTER_NAME="$CORRECT_NAME"
    export GIT_COMMITTER_EMAIL="$CORRECT_EMAIL"
fi
if [ "$GIT_AUTHOR_NAME" = "$OLD_NAME" ]
then
    export GIT_AUTHOR_NAME="$CORRECT_NAME"
    export GIT_AUTHOR_EMAIL="$CORRECT_EMAIL"
fi
' --tag-name-filter cat -- --branches --tags