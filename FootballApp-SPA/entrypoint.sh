#!/bin/bash

set -e
run_cmd="ng serve -c docker"

echo "--> Stargin Angular application..."
exec $run_cmd
