type LoginFieldProps = {
    username: string;
    addUsername: (event: React.ChangeEvent<HTMLInputElement>) => void;
    password: string;
    addPassword: (event: React.ChangeEvent<HTMLInputElement>) => void;
    logUser: (event: React.FormEvent<HTMLFormElement>) => void;
    error: string
}

function Form(props: LoginFieldProps) {
    return (
        <main className="p-4 items-center">
        <form onSubmit={props.logUser}>
        <label className="text-sm font-bold">{props.error}</label>
        <div className="mt-4">
            <input
            name= "Username"
            value={props.username}
            onInput={props.addUsername}
            placeholder="Enter your username"
            className="p-2 border-b-2 border-gray-500 w-2/3 text-center"
            />
        </div>
        <div className="mt-4">
            <input
            name="Password"
            type="password"
            value={props.password}
            onInput={props.addPassword}
            placeholder="Enter your password"
            className="p-2 border-b-2 border-gray-500 w-2/3 text-center"
            />
        <div>
        <button className="mt-4">Submit</button>
        </div>
        </div>
        </form>
        </main>
    );
}

export default Form;