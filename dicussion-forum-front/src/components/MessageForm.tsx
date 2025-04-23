import { useEffect } from "react";

type MessageFieldProps = {
  message: string;
  addMessage: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  sendMessage: (event: React.FormEvent<HTMLFormElement>) => void;
  error: string;
}

/**
 * Form to send a message to the topic
 * @param props Props for the form
 * @returns MessageForm
 */
function MessageForm (props: MessageFieldProps) {

  // Function to autosize the text area
  const autosizeTextArea = (
    textAreaRef: HTMLTextAreaElement | null,
    value: string 
  ) => {
    // eslint-disable-next-line react-hooks/rules-of-hooks
    useEffect(() => {
    if (textAreaRef) {
      textAreaRef.style.height = '0px';
      textAreaRef.style.height = `${textAreaRef.scrollHeight}px`;
    }
  }, [textAreaRef, value]);
}

  return (
    <div>
      <form onSubmit={props.sendMessage} className="w-full flex justify-center flex-col p-5">
        <label className="text-sm font-bold">{props.error}</label>
          <textarea
            name="content"
            value={props.message}
            onInput={props.addMessage}
            onChange={(e => autosizeTextArea(e.target, e.target.value))}
            placeholder="Enter your message..."
            className="text-center"
          />        
          <div>
            <button className="mt-4">Send</button>
          </div>
      </form>
    </div>
  );
}
export default MessageForm;