# Wizard-Of-Om

## The utility's job is to convert SQL queries to their SqlOm c# code equivalent. ##



How it works:

*Step 1.*

	Fill in the Sql textbox with a Sql query, and press 'Transform' at the menu bar.

*Step 2.*

	The Sql is parsed into a custom SelectQuery structure, similar to the SqlOm.SelectQuery structure.

*Step 3.*

	The structure is processed as raw c# code.

*Step 4.*

	It gets compiled as a memory assembly and using SqlOm's SqlServerRenderer, it returns the generated Sql as a string.
	
*Step 5.*

 	The SqlOm textbox gets filled with the generated SqlOm c# code.
 	The SqlOm to Sql textbox gets filled by the generated Sql, and a special(see below) comparison is done between the original Sql and the generated one.
	
*Step 6.*

        If the comparison is successful, then, a 'Success' title will be shown near the SqlOm textbox.
        If it is not the same, then a 'Warning' will be shown.
        If an error occurs at the compile step, then a 'Error' will be shown, and the SqlOm textbox will be filled with the reason/origin of the error.
	


**Note:**

The comparison has some tricky parts to take into consideration. 

- At order by, even though ASC is optional, SqlOm will output it, so it must be added for the comparison to succeed.

- SqlOm omits the 'AS' alias for tables. Thus, it must be excluded.

- Using no parentheses, and depending on the binary expression situation, the outcome may be generated with a different order than the original Sql.

- Brackets, parentheses, and chars such as multiple empty spaces are removed from both original Sql and the generated one for the comparison
