import { useEffect } from "react";

type MessageFieldProps = {
  message: string;
  addMessage: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  sendMessage: (event: React.FormEvent<HTMLFormElement>) => void;
  error: string;
}

function MessageForm (props: MessageFieldProps) {

  const autosizeTextArea = (
    textAreaRef: HTMLTextAreaElement | null,
    value: string 
  ) => {
    useEffect(() => {
    if (textAreaRef) {
      textAreaRef.style.height = '0px';
      textAreaRef.style.height = `${textAreaRef.scrollHeight}px`;
    }
  }, [textAreaRef, value]);
}

  return (
    <div className="h-auto">
      <form onSubmit={props.sendMessage}>
        <label className="text-sm font-bold">{props.error}</label>
          <textarea
            name="content"
            value={props.message}
            onInput={props.addMessage}
            onChange={(e => autosizeTextArea(e.target, e.target.value))}
            placeholder="Enter your message..."
            className="p-2 border-2 border-gray-500 w-2/3 text-center"
          />        
          <div>
            <button className="mt-4">Send</button>
          </div>
      </form>
    </div>
  );
}
export default MessageForm;