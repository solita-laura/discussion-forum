import { useEffect, useState } from "react"
import { Link, useNavigate } from "react-router-dom";
import Form from "../components/Form";
import { GetUserId } from "../functions/GetUserId";

function RegistrationScreen(){

    type RegistrationValues = {
        Username: string,
        Password: string
    }

    type ErrorValues = {Error: string}

    const [registrationValues, setRegistrationValues] = useState<RegistrationValues>({
        Username: '',
        Password: ''
    });

    const [errorValues, setErrorValues] = useState<ErrorValues>({Error: ''});
    const [succesfullRegistration, setSuccesfullRegistration] = useState<boolean>(false);

    const navigate = useNavigate();

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRegistrationValues({
            ...registrationValues,
            [event.target.name]: event.target.value
        })
    }

    const registerUser = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!registrationValues.Username || !registrationValues.Password){
            return;
        }

        if(!checkPassword(registrationValues.Password)){
            setErrorValues({Error: 'Password should contain at least one lowercase letter, one uppercase letter and one special character (!@#$%^&*) and it should be at least 8 characters long.'});
            return;
        }

        try{
            await fetch('http://localhost:5055/api/Auth/register-user', {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({Username: registrationValues.Username, Password: registrationValues.Password})
            })
            .then(response => {
                if(response.ok){
                    setErrorValues({Error: ''});
                    setSuccesfullRegistration(!succesfullRegistration);
                } else {
                    switch (response.status){
                        case 409:
                            setErrorValues({Error: "Username is already in use"});
                            return;
                        default:
                            setErrorValues({Error: 'Error in registration. Please try again.'});
                            return;
                    }
                }
            })
        }catch{
            setErrorValues({Error: "Error in registration. Please try again."})
        }
    }

    /**
     * Check if the user is already logged in and redirect to the dashboard
     */
    
    useEffect(() => {
        try{
             async function fetchUserId() {
               const id = await GetUserId();
               if (id) {
                 navigate('/dashboard');
               } 
             }
             fetchUserId();}
        catch{
            return;
            }
           }, [navigate]);
    
    /**
   * check password validation
   * @returns boolean
   */

  const checkPassword = (nameToValidate: string): boolean => {
    return RegExp(('^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*[!@#$%^&*]).{8,}$')).test(nameToValidate);
  }

    return (
        <div>
        {!succesfullRegistration ? (
            <div>
            <p>Welcome to the discussion forum!</p>
            <p>Please create a user to continue.</p>
            <div className="w-2xl">
            <Form
                username={registrationValues.Username} 
                addUsername={handleChange} 
                password={registrationValues.Password} 
                addPassword={handleChange}
                logUser={registerUser}
                error = {errorValues.Error}/>
            </div>
            </div>
            ) : 
            <div>
                <p>Your user is now created succesfully.</p>
                <p>Go back to <Link to="/login" className="underline hover:text-gray-700">login</Link></p>
            </div>
            }
        </div>
        );

}

export default RegistrationScreen;