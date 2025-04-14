import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';

type MessageProps = {
  content: string;
  upvotes: number;
};

function TopicMessage (props: MessageProps) {
  return (
    <div className="message">
      <p>{props.content}</p>
      <div className="inline-flex items-center space-x-1 p-2">
        <FavoriteBorderIcon />
        <p className="text-xs">{props.upvotes}</p>
      </div>
    </div>
  );
}
export default TopicMessage;