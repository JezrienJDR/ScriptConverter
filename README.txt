
This is a simple program that converts visual novel scripts from a custom
format to Ren'Py's format. See TestScript.txt for a sample of this custom 
format.

This format is punctuation-light, designed for ease of writing on a mobile 
device. For example, it omits the quotation marks around dialog. The program 
was initially written to add them in.

Additionally, my format has the following syntactical differences:
	
		= Custom =	= Ren'Py =
Italics: 	*Text* 		{i}Text{/i}
Dialog Wait:	~ 		{w}
Paragraph:	^ 		{p}

==============================================================================

To use the converter:

1. Run ScriptConverter.exe (double click it).
2. Hit 'Browse' to open the file explorer; navigate to the .txt file you want 
   to convert and select it. (TestScript.txt is set up for demonstration 
   purposes.)
3. Enter the names of the characters (Bob, Alice).
4. Hit 'Convert.' The converted script will appear in the same folder as the 
   original version.  

