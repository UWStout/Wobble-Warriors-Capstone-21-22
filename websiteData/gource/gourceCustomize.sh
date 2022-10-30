# Remove actions after a certain date (currently December SGX)
cat gourceLog.txt | awk -F\| '$1<=1639785600' > gourceLog.temp
sed -i.bak '/Packages/d' ./gourceLog.temp
sed -i.bak '/ProjectSettings/d' ./gourceLog.temp
sed -i.bak '/Plugins/d' ./gourceLog.temp
sed -i.bak '/Polybrush/d' ./gourceLog.temp
sed -i.bak '/TextMesh/d' ./gourceLog.temp
sed -i.bak '/\.meta/d' ./gourceLog.temp
mv gourceLog.temp gourceLog.txt
rm gourceLog.temp.bak

# Setup Project Name
projName="Wobble Warriors - Unity 3d Project"

function fix {
  sed -i -- "s/$1/$2/g" gourceLog.txt
}

# Replace non human readable names with proper ones
fix "|berriers|" "|Prof. B|"
fix "|tetzlaffm|" "|Prof. Tetzlaff|"
fix "|leclairt3344|" "|Trent LeClair|"
fix "|lavalleyb5552|" "|Olivia LaValley|"
fix "|schneiderj3185|" "|Jaden Schneider|"
fix "|hillc3327|" "|Christian Hill|"
fix "|nilesb9960|" "|Bryce Niles|"
fix "|bauchb3328|" "|Brian Bauch|"
fix "|montek7157|" "|Katherine Monte|"
fix "|thielea1319|" "|Adrianna Thiele|"
