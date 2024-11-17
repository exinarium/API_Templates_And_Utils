#!/bin/bash

DIR=$1;

if [[ -z "$DIR" ]]
then 
    DIR=. 
fi

cd $DIR
cd ../
TARGET="./TestResults";

[ -d $TARGET ] && rm -r $TARGET

dotnet test --collect:"XPlat Code Coverage" --results-directory "$TARGET"

SUBDIR=()
for FILE in $TARGET/*; do
[[ -d $FILE ]] && SUBDIR+=("$FILE")
done

~/.dotnet/tools/reportgenerator "-reports:${SUBDIR[0]}/coverage.cobertura.xml" "-targetdir:$TARGET/Report/" -reporttypes:HTML;

cd $DIR
cd $TARGET/Report/
open index.html
