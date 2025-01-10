#!/bin/bash
echo "Building..."
dotnet build DupeClear.Desktop

echo "Creating run.sh..."
script_dir="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"
echo "nohup \"${script_dir}/DupeClear.Desktop/bin/Debug/net8.0-windows/DupeClear.Desktop\" &>/dev/null &" > run.sh
chmod +x run.sh
echo "Done"
echo "Launching app..."
./run.sh
