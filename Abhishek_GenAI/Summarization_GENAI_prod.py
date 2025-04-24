import boto3
from botocore.exceptions import ClientError
import sys
import pyodbc
import json

# Suppress specific warnings
client = boto3.client("bedrock-runtime", region_name="us-east-1", aws_access_key_id="AKIAWF7TD545IFJKEZOB", aws_secret_access_key="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")
#model_id = "meta.llama2-13b-chat-v1"
model_id = "meta.llama3-70b-instruct-v1:0"
DatabaseString = sys.argv[2]

# Define the database connection parameters globally if using connection pooling
CONNECTION_STRING = (
    'DRIVER={ODBC Driver 17 for SQL Server};'
    'SERVER=EC2AMAZ-G25T1EO;'
    'DATABASE='+DatabaseString+';'
    'UID=sa;'
    'PWD=Kaseya@123;'
)

def Database_connectivity(Ticket_ID):
    # Connect to the database
    try:
        #print("database")
        with pyodbc.connect(CONNECTION_STRING) as connection:
            cursor = connection.cursor()
            query = "SELECT  NoteDesc +' '+Convert(varchar,EditedDt) as NoteDesc FROM SD_TicketNotes WHERE Ticketref=?"
            #print("before execution")
            cursor.execute(query, (Ticket_ID,))
            #print("after execution")
            row = cursor.fetchall()
            combined_string = "\n".join(" ".join(item) for item in row)
            if combined_string:
                #print (combined_string,"tytdsyugfsdgyufg")
                return combined_string
            else:
                return("No data to summarize")
                sys.exit()
    except Exception as e:
        #print(f"Database error: {e}")
        return None

def GenAI(Ticket_ID):
    TicketNote = Database_connectivity(Ticket_ID)
    #print("in GenAI function")
    text = str(TicketNote)
    text = text.replace("\n", " ").replace("\r", "")
    body = {
    "prompt": "<|begin_of_text|><|start_header_id|>user<|end_header_id|>"+text+"<|eot_id|><|start_header_id|>",
    "max_gen_len": 512,
    "temperature": 0.5,
    "top_p": 0.9


        }
    conversation = [
        {
            "role": "user",
            "content": [{"text": text}],
        }
    ]

    try:
        body_json = json.dumps(body)
        # Send the message to the model, using a basic inference configuration.
        response = client.invoke_model(
            modelId=model_id,
            contentType="application/json",
            accept="application/json",
            body=body_json
        )

        # Extract and print the response text.
        response_text = json.loads(response['body'].read().decode('utf-8'))
        #print(response_text)
        return(response_text["generation"])

    except (ClientError, Exception) as e:
        print(f"ERROR: Can't invoke '{model_id}'. Reason: {e}")
        exit(1)
if __name__ == "__main__":
    #print("intisalizing python")
    TicketID = sys.argv[1]
    #print ("In main function",TicketID)
    final_output = GenAI(TicketID)
    if final_output:
        print(final_output)
    else:
        print("No output generated from GenAI.")
    
