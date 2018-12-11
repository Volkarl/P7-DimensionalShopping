
# Servers are copied from page: https://account.ipvanish.com/index.php?t=Server%20List
# There wasn't an easy way to not hardcode this, since accessing the web page requires one to be logged in with an ipVanish account
def getServerURL(x):
    return {
        'None'                  : 'none',
    	'SouthAfrica'			: 'jnb-c01.ipvanish.com',
    	'USA'					: 'iad-a01.ipvanish.com',
    	'UnitedKingdom'			: 'lon-c12.ipvanish.com',
    	'Ukraine'				: 'iev-c01.ipvanish.com',
    	'Slovakia'				: 'bts-c01.ipvanish.com',
    	'Slovenia'				: 'lju-c03.ipvanish.com',
    	'Singapore'				: 'sin-a04.ipvanish.com',
    	'Sweden'				: 'sto-a03.ipvanish.com',
    	'Serbia'				: 'beg-c02.ipvanish.com',
    	'Romania'				: 'otp-c05.ipvanish.com',
    	'Portugal'				: 'lis-c04.ipvanish.com',
    	'Poland'				: 'waw-c02.ipvanish.com',
    	'Philippines'			: 'mnl-a01.ipvanish.com',
    	'NewZealand'			: 'akl-c09.ipvanish.com',
    	'Norway'				: 'osl-c01.ipvanish.com',
    	'Netherlands'			: 'ams-a16.ipvanish.com',
    	'Luxembourg'			: 'lux-c02.ipvanish.com',
    	'Denmark'				: 'cph-c03.ipvanish.com',
    	'Germany'				: 'fra-a06.ipvanish.com'
    }.get(x, '')    

