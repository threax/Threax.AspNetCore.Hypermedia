<?php

namespace phptest\client;
use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;

class EntryPointInjector {
    private $url;
    private $fetcher;
    private $instance = NULL;

    public function __construct(string $url, CurlHelper $fetcher) {
        $this->url = $url;
        $this->fetcher = $fetcher;
    }

    public load(): EntryPointResult {
        if ($this->$instance === NULL) {
            $this->$instance = EntryPointResult::Load($this->url, $this->fetcher);
        }

        return $this->$instance;
    }
}

class EntryPointResult {
    private $client;

    public static function Load(string $url, CurlHelper $fetcher): EntryPointResult {
        $result = HalEndpointClient::Load($url, $fetcher)
        return new EntryPointResult($result);
    }

    public function __construct(HalEndpointClient $client) {
        $this->client = $client;
    }

    public function getData() {
        return $this->client->getData();
    }

    public function refresh(): EntryPointResult {
        $r = $this->client->loadLink("self");
        return new EntryPointResult($r);
    }

    public function canRefresh(): boolean {
        return $this->client->hasLink("self");
    }

    public function linkForRefresh() {
        return $this->client->getLink("self");
    }

    public function getRefreshDocs(HalEndpointDocQuery $query = NULL) {
        return $this->client->loadLinkDoc("self", $query)->getData();
    }

    public function hasRefreshDocs(): boolean {
        return $this->client->hasLinkDoc("self");
    }
}

class HalEndpointDocQuery {
    public $includeRequest;
    public $includeResponse;
}
