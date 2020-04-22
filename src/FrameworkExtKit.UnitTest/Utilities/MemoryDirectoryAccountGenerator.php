<?php
/****************************************************
 * Created By Yufei Liu @ 2008-29 Mar 2008-19:15:05 *  
 * Contact: feilfly@gmail.com                       *
 *                                                  *    
 * Terms of Use:				    *
 * Free to use, distribute and modify under the GNU *
 * lincese. It's required to keep the oringal author*
 * information                                      *
 ****************************************************/
 
class LDAPUser{
  
	public $alias;
	public $ldap_connection;
	
	public $print_name;
	public $manager_name;
	public $manager;
	public $manager_directory_id;
	public $email;
	public $photo;
	public $gin_number;
	public $access_card_number;
	public $unique_id;
	public $directory_id;
	public $assigned_country;
	public $job_title;
	public $telephone_number;
	public $mobile_phone_number;
	public $organisation;
	public $organisational_unit;
	public $department;
	public $city;
	public $dn;
    public $geosite;
	
	public $ldap_host;
	public $ldap_port;
	public $ldap_service_account;
	public $ldap_service_account_password;
	public $ldap_base_dn;
	public $last_error;
    public $record;
	
	public function LDAPUser(){
		$this->setLDAPHost("ldap.sample.com", 389);
		$this->setBaseDN('o=sample,c=an');

		#$this->setLDAPHost("dir.sample.com", 389);
		#$this->setBaseDN('dc=dir,dc=sample,dc=com');
		#$this->setLDAPHost("oud.ldap.sample.com", 389);
		#$this->setBaseDN('o=sample,c=an');

		$this->readServiceAccount();
	}

	public function setLDAPHost($host, $port=389){
		$this->ldap_host = $host;
		$this->ldap_port = $port;

	}

	public function setBaseDN($dn){
		$this->ldap_base_dn = $dn;
	}

	public function setServiceAccount($dn, $pwd){
		$this->ldap_service_account = $dn;
		$this->ldap_service_account_password = $pwd;
	}

	function readServiceAccount(){
		$cfg_file = 'path-to-service_account.cfg';
		if(file_exists($cfg_file)){
			$string = file_get_contents($cfg_file);
			$lines = split("\n", $string);
			if(count($lines) > 2){
				$this->setServiceAccount(trim($lines[0]), trim($lines[1]));
			}
		}
	}
	
	function readUserPreference($search_key){
		if($this->ldap_connection){
			$ds = $this->ldap_connection;
			if(strlen($this->ldap_service_account) > 0){
				@ldap_bind($ds, $this->ldap_service_account, $this->ldap_service_account_password);
			}

			$sr = ldap_search($ds , $this->ldap_base_dn, $search_key);
            #print_r $search_key;
			if(!$sr){
				$this->setLastError("line ".__LINE__.": LDAP Search failed: ".ldap_error($ds));
			}else{
				$info = ldap_get_entries($ds, $sr);
                #print_r($info);
				if($info['count'] == 1){
					$record = $info[0];
                    $this->dn = $record['dn'];
                    $this->record = $record;
					$this->setPreference($record);
				}
			}
		}else{
			echo "you may forgot to call connectLDAPServer() first\n";
		}
	}
	
	public function authorizeUser($alias, $password){
		$this->readUserPreference("name=$alias");
		if($this->dn && @ldap_bind($this->ldap_connection, $this->dn, $password)){
			return true;
		}
		return false;
	}
	
	public function getManagedMembers(){
		$ds = $this->ldap_connection;
		$sr = ldap_search($ds , $this->ldap_base_dn, "manager=*{$this->directory_id}*");
		$entries = ldap_get_entries($ds, $sr);
		
		$members = array();
		
		$count = $entries['count'];
		for($i=0; $i<$count; $i++){
			$entry = $entries[$i];
			$member = new LDAPUser();
			if($this->getEntryValue($entry, 'employeetype') == 'employee'){
				$member->setPreference($entry);
				$members[$member->getAlias()] = $member;
			}
		}
		return $members;
	}
	
	public function getTeamMembers(){
		$ds = $this->ldap_connection;
		$sr = ldap_search($ds , $this->ldap_base_dn, "manager={$this->manager}");
		$entries = ldap_get_entries($ds, $sr);
		
		$members = array();
		
		$count = $entries['count'];
		for($i=0; $i<$count; $i++){
			$entry = $entries[$i];
			$member = new LDAPUser();
			if($this->getEntryValue($entry, 'employeetype') == 'employee'){
				$member->setPreference($entry);
				$members[$member->getAlias()] = $member;
			}
		}
		return $members;
	}
	
	private function savePhoto($alias, $photo){
		$digest = md5($photo);
		//$fname = "photo/$digest.jpg";
		$fname = "photo/$alias.jpg";
		
		if(!file_exists($fname)){
			$fhandle = fopen($fname, "wb");
			fwrite($fhandle, $photo);//, strlen($photo));
			fclose($fhandle);
		}
		
	}
	
	public function setPreference($record){
		$this->alias = $this->getEntryValue($record, 'alias');
		$this->print_name = $this->getEntryValue($record, 'givenname')." ".$this->getEntryValue($record, "sn");
        $this->sir_name = $this->getEntryValue($record, 'sn');
        $this->given_name = $this->getEntryValue($record, 'givenname');
		$this->email = $this->getEntryValue($record, 'mail');
		$this->directory_id = $this->getEntryValue($record, 'employeenumber');
		$this->gin_number = $this->getEntryValue($record, 'employeenumber');
		//$this->photo = $this->getEntryValue($record, 'jpegphoto');
		//$this->photo = $this->savePhoto($this->alias, $this->getEntryValue($record, 'jpegphoto'));
		#$this->photo = "http://directory.sample.com/misc/pictures/".$this->gin_number.".jpg";
		
		#$this->access_card_number = $this->getEntryValue($record, 'applicationmifareno');
		#$this->unique_id = $this->getEntryValue($record, 'uid');
		$this->assigned_country = $this->getEntryValue($record, 'c');
		#$this->job_title = $this->getEntryValue($record, 'jobtitle');
		$this->telephone_number = $this->getEntryValue($record, 'telephonenumber');
		$this->mobile_phone_number = $this->getEntryValue($record, 'mobile');
		$this->organisation = $this->getEntryValue($record, 'division');
		$this->organisational_unit = $this->getEntryValue($record, 'division');
		$this->department = $this->getEntryValue($record, 'department'); 
		$this->city = $this->getEntryValue($record, 'l'); 
		$this->dn = $this->getEntryValue($record, 'distinguishedname');
        $this->geosite = $this->getEntryValue($record, 'locationcode');
		$manager = $this->getEntryValue($record, 'manager');

        $this->orgnisation_unit = $this->getEntryValue($record, 'ou');
		
		$this->manager = $manager;
		$manager = split(",", $manager);
		$manager = $manager[0];
		$manager = str_replace("CN=","", $manager);
		
		$this->manager_name = $manager;
		$this->manager_directory_id = 0;
	}
	
	public function getEntryValue($record, $field, $pos=0){
		if(isset($record[$field])){
			 if($record[$field]['count'] > 0){
			 	return $record[$field][$pos];
			 }
		}
		return null;
	}

	public function getEntryValues($record, $field){
		if(isset($record[$field])){
			 if($record[$field]['count'] > 0){
			 	return $record[$field];
			 }
		}
		return null;
	}
	
	function connectLDAPServer(){
		if(!$this->ldap_connection){
			if($this->ldap_port == 636){
				$this->ldap_connection = ldap_connect('ldaps://'.$this->ldap_host, $this->ldap_port);
			}else{
				$this->ldap_connection = ldap_connect($this->ldap_host, $this->ldap_port);
			}
			if(!$this->ldap_connection){
				$this->setLastError(__LINE__.": connectLDAPServer - Can not connect to ldap server.");
			}
			ldap_set_option($this->ldap_connection, LDAP_OPT_PROTOCOL_VERSION, 3);
			ldap_set_option($this->ldap_connection, LDAP_OPT_REFERRALS, 0);
			ldap_set_option($this->ldap_connection, LDAP_OPT_NETWORK_TIMEOUT, 3);
			#@ldap_start_tls($this->ldap_connection);
		}
		return true;
	}
	
	function disconnectLDAPServer(){
		if($this->ldap_connection){
			ldap_close($this->ldap_connection);
			$this->ldap_connection = null;
		}
	}
	
	public function setLastError($err){
		$this->last_error = $err;
	}
	
	public function getLastError(){
		return $this->last_error;
	}
	
	/**
	 * CalendarReport::setAlias()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setAlias($value){
		$this->alias = $value;
	}

	/**
	 * CalendarReport::getAlias()
	 * 
	 * @return mix
	 */ 
	public function getAlias(){
		return $this->alias;
	}

	/**
	 * CalendarReport::setPrintName()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setPrintName($value){
		$this->print_name = $value;
	}

	/**
	 * CalendarReport::getPrintName()
	 * 
	 * @return mix
	 */ 
	public function getPrintName(){
		return $this->print_name;
	}

	/**
	 * CalendarReport::setManagerName()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setManagerName($value){
		$this->manager_name = $value;
	}

	/**
	 * CalendarReport::getManagerName()
	 * 
	 * @return mix
	 */ 
	public function getManagerName(){
		return $this->manager_name;
	}

	/**
	 * CalendarReport::setManager()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setManager($value){
		$this->manager = $value;
	}

	/**
	 * CalendarReport::getManager()
	 * 
	 * @return mix
	 */ 
	public function getManager(){
		return $this->manager;
	}

	/**
	 * CalendarReport::setManagerDirectoryId()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setManagerDirectoryId($value){
		$this->manager_directory_id = $value;
	}

	/**
	 * CalendarReport::getManagerDirectoryId()
	 * 
	 * @return mix
	 */ 
	public function getManagerDirectoryId(){
		return $this->manager_directory_id;
	}

	/**
	 * CalendarReport::setEmail()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setEmail($value){
		$this->email = $value;
	}

	/**
	 * CalendarReport::getEmail()
	 * 
	 * @return mix
	 */ 
	public function getEmail(){
		return $this->email;
	}

	/**
	 * CalendarReport::setPhoto()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setPhoto($value){
		$this->photo = $value;
	}

	/**
	 * CalendarReport::getPhoto()
	 * 
	 * @return mix
	 */ 
	public function getPhoto(){
		return $this->photo;
	}

	/**
	 * CalendarReport::setGinNumber()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setGinNumber($value){
		$this->gin_number = $value;
	}

	/**
	 * CalendarReport::getGinNumber()
	 * 
	 * @return mix
	 */ 
	public function getGinNumber(){
		return $this->gin_number;
	}

	/**
	 * CalendarReport::setAccessCardNumber()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setAccessCardNumber($value){
		$this->access_card_number = $value;
	}

	/**
	 * CalendarReport::getAccessCardNumber()
	 * 
	 * @return mix
	 */ 
	public function getAccessCardNumber(){
		return $this->access_card_number;
	}

	/**
	 * CalendarReport::setUniqueId()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setUniqueId($value){
		$this->unique_id = $value;
	}

	/**
	 * CalendarReport::getUniqueId()
	 * 
	 * @return mix
	 */ 
	public function getUniqueId(){
		return $this->unique_id;
	}

	/**
	 * CalendarReport::setDirectoryId()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setDirectoryId($value){
		$this->directory_id = $value;
	}

	/**
	 * CalendarReport::getDirectoryId()
	 * 
	 * @return mix
	 */ 
	public function getDirectoryId(){
		return $this->directory_id;
	}

	/**
	 * CalendarReport::setAssignedCountry()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setAssignedCountry($value){
		$this->assigned_country = $value;
	}

	/**
	 * CalendarReport::getAssignedCountry()
	 * 
	 * @return mix
	 */ 
	public function getAssignedCountry(){
		return $this->assigned_country;
	}

	/**
	 * CalendarReport::setJobTitle()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setJobTitle($value){
		$this->job_title = $value;
	}

	/**
	 * CalendarReport::getJobTitle()
	 * 
	 * @return mix
	 */ 
	public function getJobTitle(){
		return $this->job_title;
	}

	/**
	 * CalendarReport::setTelephoneNumber()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setTelephoneNumber($value){
		$this->telephone_number = $value;
	}

	/**
	 * CalendarReport::getTelephoneNumber()
	 * 
	 * @return mix
	 */ 
	public function getTelephoneNumber(){
		return $this->telephone_number;
	}

	/**
	 * CalendarReport::setMobilePhoneNumber()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setMobilePhoneNumber($value){
		$this->mobile_phone_number = $value;
	}

	/**
	 * CalendarReport::getMobilePhoneNumber()
	 * 
	 * @return mix
	 */ 
	public function getMobilePhoneNumber(){
		return $this->mobile_phone_number;
	}

	/**
	 * CalendarReport::setOrganisation()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setOrganisation($value){
		$this->organisation = $value;
	}

	/**
	 * CalendarReport::getOrganisation()
	 * 
	 * @return mix
	 */ 
	public function getOrganisation(){
		return $this->organisation;
	}

	/**
	 * CalendarReport::setOrganisationalUnit()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setOrganisationalUnit($value){
		$this->organisational_unit = $value;
	}

	/**
	 * CalendarReport::getOrganisationalUnit()
	 * 
	 * @return mix
	 */ 
	public function getOrganisationalUnit(){
		return $this->organisational_unit;
	}

	/**
	 * CalendarReport::setDepartment()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setDepartment($value){
		$this->department = $value;
	}

	/**
	 * CalendarReport::getDepartment()
	 * 
	 * @return mix
	 */ 
	public function getDepartment(){
		return $this->department;
	}

        /**
         * CalendarReport::setDepartment()
         *
         * @param mix $value
         * @return
         */
        public function setCity($value){
                $this->city = $value;
        }

        /**
         * CalendarReport::getDepartment()
         *
         * @return mix
         */
        public function getCity(){
                return $this->city;
        }


	/**
	 * CalendarReport::setDn()
	 * 
	 * @param mix $value
	 * @return
	 */ 
	public function setDn($value){
		$this->dn = $value;
	}

	/**
	 * CalendarReport::getDn()
	 * 
	 * @return mix
	 */ 
	public function getDn(){
		return $this->dn;
	}

	public function getLDAPConnection(){
		return $this->ldap_connection;
	}
  
}
?>
<?php
function combine($values){
    if($values == null){
        return '';
    }
    array_shift($values);
    return '"'.join($values, '", "').'"';
}
$ldap_user = new LDAPUser();
$ldap_user->connectLDAPServer();


$users = array(
    'LYufei'
);

$alias_param = $argv[1];

if($alias_param){
    $users = array(
        $alias_param
    );
}

for($i=0; $i<count($users); $i++){

$alias = $users[$i];

$ldap_user->setAlias(null);
$ldap_user->readUserPreference("(alias=$alias)");

/*
$gin_numbers = array(
    '00000000'
);

for($i=0;$i<count($gin_numbers); $i++){
    $gin = $gin_numbers[$i];

    $ldap_user->readUserPreference("(alias=KLi5)");

    if($ldap_user->getAlias() == null){
        echo $ldap_user->organisational_unit."\n";
        echo "$g - No\n";
    }else{
        echo "$g -> ".$ldap_user->getAlias()." - Yes\n";
    }
}
 */

$record = $ldap_user->record;

echo '
            #region '.$ldap_user->getAlias().'
            public static DirectoryAccount '.$ldap_user->getAlias().' = new MemoryDirectoryAccount(){
                    ObjectClass = "'.$ldap_user->getEntryValue($record, 'objectclass').'",
                    Alias = "'.$ldap_user->getAlias().'", 
                    SirName = "'.$ldap_user->getEntryValue($record, 'sn').'",
                    GivenName = "'.$ldap_user->getEntryValue($record, 'givenname').'",
                    DisplayName = "'.$ldap_user->getEntryValue($record, 'displayname').'",
                    EmployeeType = "'.$ldap_user->getEntryValue($record, 'employeetype').'",
                    BusinessCategory = "'.$ldap_user->getEntryValue($record, 'businesscategory').'",
                    AccessRights = new string[]{'.combine($ldap_user->getEntryValues($record, 'accessrights')).'},
                    CommonName = "'.$ldap_user->getEntryValue($record, 'commonname').'",
                    ActiveDirectoryDN = "'.$ldap_user->getEntryValue($record, 'activedirectorydn').'",
                    City = "'.$ldap_user->getEntryValue($record, 'l').'",
                    Country="'.$ldap_user->getEntryValue($record, 'c').'",
                    Department = "'.$ldap_user->getEntryValue($record, 'department').'",
                    DistinguishName = "'.$record['dn'].'",
                    GIN = "'.$ldap_user->getEntryValue($record, 'employeenumber').'",
                    Id = '.$ldap_user->getEntryValue($record, 'id').',
                    JobCategory = "'.$ldap_user->getEntryValue($record, 'jobcategory').'",
                    JobCode = "'.$ldap_user->getEntryValue($record, 'jobcode').'",
                    JobGroup = "'.$ldap_user->getEntryValue($record, 'jobgroup').'",
                    JobTitle = "'.$ldap_user->getEntryValue($record, 'jobtitle').'",
                    LocationCode = "'.$ldap_user->getEntryValue($record, 'locationcode').'",
                    Mail = "'.$ldap_user->getEntryValue($record, 'mail').'",
                    ManagerDNs = new string[] { '.combine($ldap_user->getEntryValues($record, 'manager')).'},    
                    EDMWorkStations = new string[] {'.combine($ldap_user->getEntryValues($record, 'edmworkstation')).'},
                    OrganizationName = "'.$ldap_user->getEntryValue($record, 'organizationname').'",
                    OrganizationUnit = "'.$ldap_user->getEntryValue($record, 'organizationalunitname').'",
                    PostalCode="'.$ldap_user->getEntryValue($record, 'postalcode').'",
                    Street = "'.$ldap_user->getEntryValue($record, 'street').'",
                    TelephoneNumber = "'.$ldap_user->getEntryValue($record, 'telephonenumber').'", 
                    UserId = "'.$ldap_user->getEntryValue($record, 'userid').'",
                    ITBuilding = "'.$ldap_user->getEntryValue($record, 'itbuilding').'"
            };
            #endregion';
}



$ldap_user->disconnectLDAPServer();
?>
