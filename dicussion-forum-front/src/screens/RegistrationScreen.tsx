import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom";
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

        console.log(registrationValues.Password);

        if(!checkPassword(registrationValues.Password)){
            setErrorValues({Error: 'Password should contain at least one lowercase letter, one uppercase letter and one special character (!@#$%^&*) and it should be between 8 to 20 characters long.'});
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
                    navigate('/login')
                } else {
                    setErrorValues({Error: 'Error in registration. Please try again.'})
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
             async function fetchUserId() {
               const id = await GetUserId();
               if (id) {
                 navigate('/dashboard');
               } 
             }
             fetchUserId();
           }, [navigate]);
    
    /**
   * check password validation
   * @returns boolean
   */

  const checkPassword = (nameToValidate: string): boolean => {
    return RegExp(('^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*[!@#$%^&*]).{8,20}$')).test(nameToValidate);
  }

    return (
        <div>
            <p>Welcome to the discussion forum!</p>
            <p>Please create a user to continue.</p>
            <Form 
                username={registrationValues.Username} 
                addUsername={handleChange} 
                password={registrationValues.Password} 
                addPassword={handleChange}
                logUser={registerUser}
                error = {errorValues.Error}/>
            </div>
        );

}

export default RegistrationScreen;