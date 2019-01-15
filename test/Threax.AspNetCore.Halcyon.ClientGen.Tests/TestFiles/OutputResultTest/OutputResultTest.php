use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;

class OutputResult {
    private $client

    public function __construct(HalEndpointClient $client) {
        $this->client = $client;
    }

    public function getData() {
        return $this->client->getData();
    }

    public function save($data): OutputResult {
        $r = $this->client->loadLinkWithData("Save", $data)
        return new OutputResult($r);
    }

    public function canSave(): boolean {
        return $this->client->hasLink("Save");
    }

    public function linkForSave(): hal.HalLink {
        return $this->client->getLink("Save");
    }

    public function getSaveDocs(HalEndpointDocQuery $query = NULL) {
        return $this->client->loadLinkDoc("Save", $query)->getData();
    }

    public function hasSaveDocs(): boolean {
        return $this->client->hasLinkDoc("Save");
    }
}

class HalEndpointDocQuery {
    public $includeRequest;
    public $includeResponse;
}
