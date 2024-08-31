# Messages

| Name  | Description | 
|----------|----------|
| `DecoderConfigurationUpdateMessage`    | The message DecoderConfigurationUpdateMessage is sent, when the decoder configuration has been updated.   |
| `DecoderSpecificationUpdatedMessage`  | The message DecoderSpecificationUpdatedMessage is sent, when the decoder specification has been changed.  |


# Message handler

| Name  | Description | 
|----------|----------|
| `OnGetDecoderConfiguration`    | The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received. OnGetDecoderConfiguration updates the local variables with the new decoder configuration.   |
| `OnGetDataFromDecoderSpecification`  | The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received. OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.  |
